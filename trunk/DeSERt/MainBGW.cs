using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using DeSERt;
using Gtk;
using ICSharpCode.SharpZipLib.Zip;

public partial class DeSERtMain
{

    #region DoWork

    private void RTBackground_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            ProcessStartInfo RTStartInfo = new ProcessStartInfo();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { RTStartInfo = new ProcessStartInfo("\"" + MySettings.ProgramPath + "\"", e.Argument.ToString()); }
            else if (Environment.OSVersion.Platform == PlatformID.Unix) { RTStartInfo = new ProcessStartInfo("rawtherapee", e.Argument.ToString()); }
            else if (Environment.OSVersion.Platform == PlatformID.MacOSX) { RTStartInfo = new ProcessStartInfo("rawtherapee", e.Argument.ToString()); }
            else { e.Result = InfoType.InvalidOS; return; }

            RTStartInfo.UseShellExecute = false;
            RTStartInfo.CreateNoWindow = true;

            RT.StartInfo = RTStartInfo;
            RT.Start();
            lastTime = DateTime.Now.Ticks;
            RT.WaitForExit();

            e.Result = InfoType.OK;
        }
        catch (Exception ex)
        {
            ThreadException = ex;
            e.Result = InfoType.Error;
        }
    }

    private void ExiftoolBackground_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            #region Getting Thumbnails with Exiftool

            string ExiftoolName = "";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe"); }
            else if (Environment.OSVersion.Platform == PlatformID.Unix) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool"); }
            else if (Environment.OSVersion.Platform == PlatformID.MacOSX) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool"); }       //Not sure if that works
            else { e.Result = InfoType.InvalidOS; return; }
            ProcessStartInfo exiftoolStartInfo = new ProcessStartInfo(ExiftoolName, e.Argument.ToString());
            exiftoolStartInfo.UseShellExecute = false;
            exiftoolStartInfo.CreateNoWindow = true;

            exiftool.StartInfo = exiftoolStartInfo;

            if ((string)e.Argument != "NoExif")
            {
                exiftool.Start();
                exiftool.WaitForExit();
            }

            #endregion Getting Thumbnails with Exiftool

            if (ProcessCancelled == true) { e.Result = InfoType.ProcessCancelled; return; }

            #region Getting Thumbnails manually

            //check if exiftool could extract thumbs, if not try to do it manually (works only with jpg, tiff, bmp, etc. no raw!)
            int thumbcount = Directory.GetFiles(Thumbpath).Length;

            if (AllFiles.Count != thumbcount && ProcessCancelled == false)
            {
                ProgressFileWatcher.EnableRaisingEvents = false;

                try
                {
                    FinishedThreads = new int[] { 0, 0, 0, 0, 0 };
                    WorkerHasError = false;
                    CalcWorker.Clear();
                    for (int i = 0; i < MySettings.Threads; i++)
                    {
                        CalcWorker.Add(new BackgroundWorker());
                        CalcWorker[i].WorkerSupportsCancellation = true;
                        CalcWorker[i].WorkerReportsProgress = true;
                        CalcWorker[i].DoWork += new DoWorkEventHandler(ThumbExtract_DoWork);
                        CalcWorker[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(ThumbExtract_RunWorkerCompleted);
                        CalcWorker[i].ProgressChanged += new ProgressChangedEventHandler(General_ProgressChanged);
                        CalcWorker[i].RunWorkerAsync(i);
                    }

                    for (int i = 0; i < CalcWorker.Count; i++)
                    {
                        while (CalcWorker[i].IsBusy)
                        {
                            if (ProcessCancelled == false) { Thread.Sleep(20); }
                            else { CalcWorker[i].CancelAsync(); }
                        }
                    }
                }
                catch { e.Result = InfoType.ExiftoolThumbError; return; }
            }

            #endregion Getting Thumbnails manually

            if (ProcessCancelled == true) { e.Result = InfoType.ProcessCancelled; return; }

            ExiftoolBackground.ReportProgress(1, "ChangeState");

            #region Getting the Camera Data

            exiftoolStartInfo.Arguments = "-s -ApertureValue -ShutterSpeedValue -ISO -WB_RGGBLevelsAsShot -ColorSpace -ImageSize " + Workpath;
            exiftoolStartInfo.RedirectStandardOutput = true;

            exiftool.StartInfo = exiftoolStartInfo;
            exiftool.Start();

            string CameraData = exiftool.StandardOutput.ReadToEnd();

            if (!String.IsNullOrEmpty(CameraData))
            {
                FinishedThreads = new int[] { 0, 0, 0, 0, 0 };
                ExifWorker.WorkerSupportsCancellation = true;
                ExifWorker.WorkerReportsProgress = false;
                ExifWorker.DoWork += new DoWorkEventHandler(ExifCalc_DoWork);
                ExifWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExifCalc_RunWorkerCompleted);
                ExifWorker.RunWorkerAsync(CameraData);

                while (ExifWorker.IsBusy)
                {
                    if (ProcessCancelled == false) { Thread.Sleep(20); }
                    else { ExifWorker.CancelAsync(); }
                }
            }
            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();

            #endregion Getting the Camera Data

            e.Result = InfoType.OK;
        }
        catch (Exception ex)
        {
            ThreadException = ex;
            e.Result = InfoType.Error;
        }
    }

    private void ThumbExtract_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            int Nr = (int)e.Argument;
            int start = Nr * (AllFiles.Count / MySettings.Threads);
            int end = start + (AllFiles.Count / MySettings.Threads);
            if (Nr == MySettings.Threads - 1) { end = AllFiles.Count; }

            for (int i = start; i < end; i++)
            {
                if (CalcWorker[(int)e.Argument].CancellationPending) { e.Result = InfoType.ProcessCancelled; return; }

                System.Drawing.Image img = System.Drawing.Image.FromFile(AllFiles[i].FilePath);
                double factor = (double)img.Width / (double)img.Height;

                System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                System.Drawing.Image thumb = img.GetThumbnailImage(160, (int)(160 / factor), myCallback, IntPtr.Zero);
                string imgname = System.IO.Path.GetFileNameWithoutExtension(AllFiles[i].Filename);
                string savepath = System.IO.Path.Combine(Thumbpath, imgname + "_Thumb.jpg");
                thumb.Save(savepath);
                img.Dispose();
                thumb.Dispose();

                if (Nr == 0) { CalcWorker[0].ReportProgress((i * MySettings.Threads) / AllFiles.Count); }
            }
            e.Result = InfoType.OK;
        }
        catch (OutOfMemoryException)
        {
            e.Result = InfoType.OpenImgError;
        }
        catch (Exception ex)
        {
            e.Result = InfoType.Error;
            ThreadException = ex;
        }
    }

    //wb steps missing
    private void ExifCalc_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            string CameraData = e.Argument.ToString();
            string[] lines = CameraData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
            string TVtmp;
            string AVtmp;
            string SVtmp;
            double TVdenom;
            double TVnume;
            int f = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                if (ExifWorker.CancellationPending == false)
                {
                    string startline = String.Empty;
                    if (f < AllFiles.Count - 1) { startline = "======== " + AllFiles[f + 1].FilePath.ToLower().Replace(@"\", "/"); }

                    if (startline == lines[i].ToLower()) { f++; }
                    else if (lines[i].ToLower().StartsWith("aperturevalue"))
                    {
                        AVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);
                        if (AVtmp.Contains("$"))
                        {
                            AllFiles[f].Av = 0;
                            AllFiles[f].AvString = "N/A";
                        }
                        else
                        {
                            AllFiles[f].AvString = AVtmp;
                            AllFiles[f].Av = Convert.ToDouble(AVtmp.Replace(".", ","));
                            AllFiles[f].Av = Math.Log(Math.Pow(AllFiles[f].Av, 2), 2);
                        }
                    }
                    else if (lines[i].ToLower().StartsWith("shutterspeedvalue"))
                    {
                        TVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);
                        if (String.IsNullOrEmpty(TVtmp)) { AllFiles[f].TvString = "N/A"; }
                        else { AllFiles[f].TvString = TVtmp; }
                        if (TVtmp.Contains(@"/"))
                        {
                            TVnume = Convert.ToDouble(TVtmp.Substring(0, TVtmp.IndexOf(@"/")));
                            TVdenom = Convert.ToDouble(TVtmp.Substring(TVtmp.IndexOf(@"/") + 1));
                            AllFiles[f].Tv = TVnume / TVdenom;
                        }
                        else
                        {
                            AllFiles[f].Tv = Convert.ToDouble(TVtmp.Replace(".", ","));
                        }
                        AllFiles[f].Tv = Math.Log(1 / AllFiles[f].Tv, 2);
                    }
                    else if (lines[i].ToLower().StartsWith("iso"))
                    {
                        SVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);
                        if (String.IsNullOrEmpty(SVtmp)) { AllFiles[f].SvString = "N/A"; }
                        else { AllFiles[f].SvString = SVtmp; }
                        AllFiles[f].Sv = Math.Log(Convert.ToInt32(SVtmp), 3.125f);
                    }
                    else if (lines[i].ToLower().StartsWith("wb_rggblevelsasshot"))
                    {
                        string[] WBtmp = lines[i].Substring(lines[i].IndexOf(":") + 2).Split(' ');
                        int[] WBvals = new int[4] { Convert.ToInt32(WBtmp[0]), Convert.ToInt32(WBtmp[1]), Convert.ToInt32(WBtmp[2]), Convert.ToInt32(WBtmp[3]) };
                        //calc the resulting brightness steps and write to AllFiles[f].WB
                    }
                    else if (lines[i].ToLower().StartsWith("colorspace"))
                    {
                        AllFiles[f].ColorSpace = lines[i].Substring(lines[i].IndexOf(":") + 2);
                    }
                    else if (lines[i].ToLower().StartsWith("imagesize"))
                    {
                        string[] tmp = lines[i].Substring(lines[i].IndexOf(":") + 2).Split('x');
                        AllFiles[f].Width = Convert.ToInt32(tmp[0]);
                        AllFiles[f].Height = Convert.ToInt32(tmp[1]);
                    }

                    if (f >= 0 && f < AllFiles.Count)
                    {
                        if (AllFiles[f].AvString == "N/A" && AllFiles[f].TvString == "N/A" && AllFiles[f].SvString == "N/A") { AllFiles[f].HasExif = false; }
                        else { AllFiles[f].HasExif = true; }

                        //Brightness Value
                        AllFiles[f].Bv = AllFiles[f].Av + AllFiles[f].Tv - AllFiles[f].Sv;
                    }

                }
                else { e.Result = InfoType.ProcessCancelled; return; }
            }

            e.Result = InfoType.OK;
        }
        catch (Exception ex)
        {
            e.Result = InfoType.Error;
            ThreadException = ex;
        }
    }

    private void StaticMaskBackgroundV21_DoWork(object sender, DoWorkEventArgs e)
    {
        //For static Camera V2.1 (and maybe also works for slowly moving camera)
        try
        {
            //Get all Files that are in the Thumb directory
            string[] files = Directory.GetFiles(Thumbpath, "*.jpg");
            int filecount = AllFiles.Count;
            CalcState Status = CalcState.Mask;

            int Nr = (int)e.Argument;
            int start = 0;
            int end = 0;
            int threads = filecount / 3;

            if (threads > MySettings.Threads) { threads = MySettings.Threads; }
            if (Nr > threads - 1) { e.Result = InfoType.None; return; }

            start = Nr * (filecount / threads);
            end = start + (filecount / threads);
            if (Nr == threads - 1) { end = filecount; }
            
            int ThumbWidth = AllFiles.Min(w => w.ThumbWidth);
            int ThumbHeight = AllFiles.Min(h => h.ThumbHeight);

            Bitmap BrMask = new Bitmap(ThumbWidth, ThumbHeight);
            
            double[,] BrightChangeMask = new double[ThumbWidth, ThumbHeight];
            bool[,] NonUseMask = new bool[ThumbWidth, ThumbHeight];

            FinishedThreads[0]++;
            while (FinishedThreads[0] != threads) { Thread.Sleep(10); }

            for (int f = start; f < end; f += 2)
            {
                if (CalcWorker[Nr].CancellationPending == true)
                {
                    if (Nr == 0) { e.Result = InfoType.Cancel; }
                    else { e.Result = InfoType.None; }
                    return;
                }

                if (f == 0) { f++; }
                if (f + 2 >= filecount) { f = filecount - 2; }

                #region Mask

                #region Assigning some variables

                System.Drawing.Image orig = System.Drawing.Image.FromFile(files[f - 1]);
                Bitmap bmp1 = LinearBitmap.CreateLinearBitmap(new Bitmap(orig, ThumbWidth, ThumbHeight), AllFiles[f - 1].ColorSpace);
                orig = System.Drawing.Image.FromFile(files[f]);
                Bitmap bmp2 = LinearBitmap.CreateLinearBitmap(new Bitmap(orig, ThumbWidth, ThumbHeight), AllFiles[f].ColorSpace);
                orig = System.Drawing.Image.FromFile(files[f + 1]);
                Bitmap bmp3 = LinearBitmap.CreateLinearBitmap(new Bitmap(orig, ThumbWidth, ThumbHeight), AllFiles[f + 1].ColorSpace);

                orig.Dispose();

                PixelFormat pf = bmp1.PixelFormat;
                System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, bmp1.Width, bmp1.Height);

                BitmapData bmd1 = bmp1.LockBits(rec, ImageLockMode.ReadOnly, pf);
                BitmapData bmd2 = bmp2.LockBits(rec, ImageLockMode.ReadOnly, pf);
                BitmapData bmd3 = bmp3.LockBits(rec, ImageLockMode.ReadOnly, pf);

                int PixelSize;
                if (pf == PixelFormat.Format16bppArgb1555 || pf == PixelFormat.Format16bppGrayScale || pf == PixelFormat.Format16bppRgb555 || pf == PixelFormat.Format16bppRgb565)
                { PixelSize = 2; }
                else if (pf == PixelFormat.Format24bppRgb)
                { PixelSize = 3; }
                else if (pf == PixelFormat.Format32bppArgb || pf == PixelFormat.Format32bppPArgb || pf == PixelFormat.Format32bppRgb)
                { PixelSize = 4; }
                else if (pf == PixelFormat.Format48bppRgb)
                { PixelSize = 6; }
                else if (pf == PixelFormat.Format64bppArgb || pf == PixelFormat.Format64bppPArgb)
                { PixelSize = 8; }
                else if (pf == PixelFormat.Format8bppIndexed)
                { PixelSize = 1; }
                else { throw new Exception("unsupported imageformat"); }

                double maxiBrDiff = 0;
                int index = 0;
                int count = 0;

                #endregion Assigning some variables

                unsafe
                {
                    for (int y = 0; y < ThumbHeight; y++)
                    {
                        byte* row1 = (byte*)bmd1.Scan0 + (y * bmd1.Stride);
                        byte* row2 = (byte*)bmd2.Scan0 + (y * bmd2.Stride);
                        byte* row3 = (byte*)bmd3.Scan0 + (y * bmd3.Stride);

                        for (int x = 0; x < ThumbWidth; x++)
                        {
                            index = x * PixelSize;

                            /*double br1 = Math.Sqrt(Math.Pow(row1[index + 2], 2) * 0.241 + Math.Pow(row1[index + 1], 2) * 0.691 + Math.Pow(row1[index], 2) * 0.068);
                            double br2 = Math.Sqrt(Math.Pow(row2[index + 2], 2) * 0.241 + Math.Pow(row2[index + 1], 2) * 0.691 + Math.Pow(row2[index], 2) * 0.068);
                            double br3 = Math.Sqrt(Math.Pow(row3[index + 2], 2) * 0.241 + Math.Pow(row3[index + 1], 2) * 0.691 + Math.Pow(row3[index], 2) * 0.068);*/

                            double br1 = (row1[index + 2] + row1[index + 1] + row1[index]) / 3;
                            double br2 = (row2[index + 2] + row2[index + 1] + row2[index]) / 3;
                            double br3 = (row3[index + 2] + row3[index + 1] + row3[index]) / 3;

                            int min = 5;
                            int max = 250;

                            if (br1 > min && br2 > min && br3 > min && br1 < max && br2 < max && br3 < max)
                            {
                                NonUseMask[x, y] = false;
                            }
                            else { NonUseMask[x, y] = true; }
                        }
                    }

                    for (int y = 0; y < ThumbHeight; y++)
                    {
                        for (int x = 0; x < ThumbWidth; x++)
                        {
                            if (NonUseMask[x, y] == false)
                            {
                                count = 0;
                                List<double> brightnessDiff1 = new List<double>();
                                List<double> brightnessDiff2 = new List<double>();
                                
                                for (int yS = -1; yS <= 1; yS++)
                                {
                                    if (y + yS < ThumbHeight && y + yS >= 0)
                                    {
                                        byte* row1 = (byte*)bmd1.Scan0 + ((y + yS) * bmd1.Stride);
                                        byte* row2 = (byte*)bmd2.Scan0 + ((y + yS) * bmd2.Stride);
                                        byte* row3 = (byte*)bmd3.Scan0 + ((y + yS) * bmd3.Stride);

                                        for (int xS = -1; xS <= 1; xS++)
                                        {
                                            if (x + xS < ThumbWidth && x + xS >= 0)
                                            {
                                                if (NonUseMask[x + xS, y + yS] == false)
                                                {
                                                    index = (x + xS) * PixelSize;
                                                    
                                                    /*double br1 = Math.Sqrt(Math.Pow(r1, 2) * 0.241 + Math.Pow(g1, 2) * 0.691 + Math.Pow(b1, 2) * 0.068);
                                                    double br2 = Math.Sqrt(Math.Pow(r2, 2) * 0.241 + Math.Pow(g2, 2) * 0.691 + Math.Pow(b2, 2) * 0.068);
                                                    double br3 = Math.Sqrt(Math.Pow(r3, 2) * 0.241 + Math.Pow(g3, 2) * 0.691 + Math.Pow(b3, 2) * 0.068);*/

                                                    double br1 = (row1[index + 2] + row1[index + 1] + row1[index]) / 3;
                                                    double br2 = (row2[index + 2] + row2[index + 1] + row2[index]) / 3;
                                                    double br3 = (row3[index + 2] + row3[index + 1] + row3[index]) / 3;
                                                    
                                                    brightnessDiff1.Add(Math.Abs(br1 - br2));
                                                    brightnessDiff2.Add(Math.Abs(br2 - br3));
                                                    
                                                    count++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (count > 0)
                                {
                                    BrightChangeMask[x, y] = Math.Max(brightnessDiff1.Average(), brightnessDiff2.Average());
                                }
                                else { BrightChangeMask[x, y] = 0; }

                                if (maxiBrDiff < BrightChangeMask[x, y]) { maxiBrDiff = BrightChangeMask[x, y]; }
                            }
                            else { BrightChangeMask[x, y] = 0; }
                        }
                    }
                }

                double newBr = 0;
                int isdark = 0;

                for (int y = 0; y < ThumbHeight; y++)
                {
                    for (int x = 0; x < ThumbWidth; x++)
                    {
                        if (NonUseMask[x, y] == false)
                        {
                            if (maxiBrDiff > 0)
                            {
                                newBr = (BrightChangeMask[x, y] * 100) / maxiBrDiff;
                                if (newBr > 100) { newBr = 100; }
                                BrightChangeMask[x, y] = Math.Abs(newBr - 100);
                            }
                            else { BrightChangeMask[x, y] = 100; }
                        }
                        else { BrightChangeMask[x, y] = 0; }

                        if (BrightChangeMask[x, y] < 0.5) { isdark++; }
                    }
                }

                if ((isdark * 100) / (ThumbHeight * ThumbWidth) > 97)
                {
                    e.Result = InfoType.TooDark;
                    return;
                }

                #endregion Mask

                #region Brightness

                count = 0;
                double[][] PixelBrightness = new double[ThumbWidth * ThumbHeight][];
                
                unsafe
                {
                    for (int y = 0; y < ThumbHeight; y++)
                    {
                        byte* row1 = (byte*)bmd1.Scan0 + (y * bmd1.Stride);
                        byte* row2 = (byte*)bmd2.Scan0 + (y * bmd2.Stride);
                        byte* row3 = (byte*)bmd3.Scan0 + (y * bmd3.Stride);

                        for (int x = 0; x < ThumbWidth; x++)
                        {
                            index = x * PixelSize;
                            
                            /*double br1 = Math.Sqrt(Math.Pow(r1, 2) * 0.241 + Math.Pow(g1, 2) * 0.691 + Math.Pow(b1, 2) * 0.068);
                            double br2 = Math.Sqrt(Math.Pow(r2, 2) * 0.241 + Math.Pow(g2, 2) * 0.691 + Math.Pow(b2, 2) * 0.068);
                            double br3 = Math.Sqrt(Math.Pow(r3, 2) * 0.241 + Math.Pow(g3, 2) * 0.691 + Math.Pow(b3, 2) * 0.068);*/

                            double br1 = (row1[index + 2] + row1[index + 1] + row1[index]) / 3;
                            double br2 = (row2[index + 2] + row2[index + 1] + row2[index]) / 3;
                            double br3 = (row3[index + 2] + row3[index + 1] + row3[index]) / 3;

                            double factor = BrightChangeMask[x, y] / 100;
                            PixelBrightness[count] = new double[3] { br1 * factor, br2 * factor, br3 * factor };
                            count++;
                        }
                    }
                }

                bmp1.UnlockBits(bmd1);
                bmp2.UnlockBits(bmd2);
                bmp3.UnlockBits(bmd3);
                bmp1.Dispose();
                bmp2.Dispose();
                bmp3.Dispose();

                double val0 = (PixelBrightness.Average(p => p[0]));
                double val1 = (PixelBrightness.Average(p => p[1]));
                double val2 = (PixelBrightness.Average(p => p[2]));
                
                if (f == 1) { AllFiles[0].Brightness = val0; }
                while (double.IsNaN(AllFiles[f - 1].Brightness)) { Thread.Sleep(10); }
                double val1P = (val1 * 100) / val0;
                AllFiles[f].Brightness = (AllFiles[f - 1].Brightness * val1P) / 100;
                double val2P = (val2 * 100) / val0;
                AllFiles[f + 1].Brightness = (AllFiles[f - 1].Brightness * val2P) / 100;

                #endregion Brightness
                
                if (Nr == 0) { CalcWorker[0].ReportProgress((int)(((((double)f * (double)threads) / (double)filecount)) * 100f), Status); }
            }

            FinishedThreads[1]++;
            while (FinishedThreads[1] != threads)
            {
                if (ProcessCancelled) { goto ProcessEnd; }
                else { Thread.Sleep(10); }
            }

            #region Statistics/BV-Values Check

            if (Nr == 0)
            {
                /*Status = CalcState.Statistics;

                for (int f = 1; f < filecount; f++)
                {
                    if (AllFiles[f].HasExif)
                    {
                        double EvP = (Math.Min(AllFiles[f].Brightness, AllFiles[f - 1].Brightness) * 100) / Math.Max(AllFiles[f].Brightness, AllFiles[f - 1].Brightness);
                        double bv1 = Math.Abs(AllFiles[f - 1].Bv);
                        double bv2 = Math.Abs(AllFiles[f].Bv);
                        double BvP = (Math.Min(bv1, bv2) * 100) / Math.Max(bv1, bv2);
                        if (bv1 < bv2) { BvP *= -1; }
                        double res = BvP - EvP;
                        if (AllFiles[f].Brightness < AllFiles[f - 1].Brightness) { res *= -1; }
                        AllFiles[f].StatisticalError = res;
                    }
                    else
                    {
                        //do statistical check
                    }

                    CalcWorker[0].ReportProgress((int)((((double)f / (double)filecount)) * 100f), Status);
                }*/
            }

            #endregion Statistics/BV-Values Check

            ProcessEnd:
            if (Nr == 0) { e.Result = InfoType.OK; }
            else { e.Result = InfoType.None; }
        }
        catch (Exception ex)
        {
            ThreadException = ex;
            e.Result = InfoType.Error;
        }
    }

    private void SaveBackground_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            string tmpdirectorypath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "tmp") + System.IO.Path.DirectorySeparatorChar;

            using (FileStream tempFileStream = new FileStream(ProjectSavePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (ZipOutputStream zipOutput = new ZipOutputStream(tempFileStream))
                {
                    // Zip with highest compression.
                    zipOutput.SetLevel(9);
                    zipOutput.SetComment(ProjectInfo.FileVersion.ToString());

                    //Project Data
                    string projfilename = SaveProject(tmpdirectorypath);
                    using (FileStream fileStream = new FileStream(projfilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read full stream to in-memory buffer.
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Length);

                        // Create a new entry for the current file.
                        ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(projfilename));
                        entry.DateTime = DateTime.Now;
                        entry.Size = fileStream.Length;
                        entry.Comment = "Main";
                        fileStream.Close();
                        zipOutput.PutNextEntry(entry);
                        zipOutput.Write(buffer, 0, buffer.Length);
                        buffer = null;
                    }

                    //Thumbs
                    string[] thumbs = Directory.GetFiles(Thumbpath);
                    for (int i = 0; i < thumbs.Length; i++)
                    {
                        using (FileStream fileStream = new FileStream(thumbs[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            // Read full stream to in-memory buffer.
                            byte[] buffer = new byte[fileStream.Length];
                            fileStream.Read(buffer, 0, buffer.Length);

                            // Create a new entry for the current file.
                            ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(thumbs[i]));
                            entry.DateTime = DateTime.Now;
                            entry.Size = fileStream.Length;
                            entry.Comment = "Thumb";
                            fileStream.Close();
                            zipOutput.PutNextEntry(entry);
                            zipOutput.Write(buffer, 0, buffer.Length);
                            buffer = null;
                        }
                    }

                    zipOutput.Finish();
                    zipOutput.Flush();
                    zipOutput.Close();
                }
            }

            Directory.Delete(tmpdirectorypath, true);

            ProjectSaved = true;
            e.Result = true;
        }
        catch (Exception ex)
        {
            e.Result = false;
            ThreadException = ex;
        }
    }

    private void OpenBackground_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            using (FileStream stream = new FileStream(ProjectSavePath, FileMode.Open))
            {
                ZipFile zippy = new ZipFile(stream);

                //File (*.depro) Version
                int Fileversion = Convert.ToInt32(zippy.ZipFileComment);
                if (Fileversion == 2) { OpenProjectV2(zippy); }
                else
                {
                    ThreadException = new NotSupportedException("This fileversion is not supported! Sorry!");
                    e.Result = false;
                    return;
                }
            }

            //checking for missing files: (make option to search for it later)
            bool filenotfound = false;
            for (int i = 0; i < AllFiles.Count; i++)
            {
                if (File.Exists(AllFiles[i].FilePath) == false){ filenotfound = true; break; }
            }
            if (filenotfound == true)
            {
                ThreadException = new FileNotFoundException();
                e.Result = false;
                return;
            }

            e.Result = true;
        }
        catch (Exception ex)
        {
            e.Result = false;
            ThreadException = ex;
        }
    }

    //command probably not correct
    private void XMPWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            string ExiftoolName = "";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe"); }
            else if (Environment.OSVersion.Platform == PlatformID.Unix) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool"); }
            else if (Environment.OSVersion.Platform == PlatformID.MacOSX) { ExiftoolName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool"); }       //Not sure if that works
            else { e.Result = InfoType.InvalidOS; return; }
            ProcessStartInfo exiftoolStartInfo = new ProcessStartInfo(ExiftoolName, e.Argument.ToString());
            exiftoolStartInfo.UseShellExecute = false;
            exiftoolStartInfo.CreateNoWindow = true;

            exiftool.StartInfo = exiftoolStartInfo;

            exiftoolStartInfo.Arguments = "-s -xmp " + Workpath;
            exiftoolStartInfo.RedirectStandardOutput = true;

            exiftool.StartInfo = exiftoolStartInfo;
            exiftool.Start();

            string XMPstuff = exiftool.StandardOutput.ReadToEnd();

            ProcessXMPString(XMPstuff);

            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();

            e.Result = InfoType.OK;
        }
        catch (Exception ex)
        {
            e.Result = InfoType.Error;
            ThreadException = ex;
        }
    }

    #endregion DoWork


    #region Completed

    private void RTBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                ProgressPulse.Stop();
                BackCounter.Stop();
                ProgressFileWatcher.EnableRaisingEvents = false;

                if (ProgState == ProgramState.PreviewRender)
                {
                    ProgState = ProgramState.Idle;

                    if ((InfoType)e.Result == InfoType.OK)
                    {
                        if (ProcessCancelled == true) { UpdateInfo(InfoType.RTWorking, 2); }
                        else
                        {
                            BrightnessPreview prevWindow = new BrightnessPreview();
                            prevWindow.ShowNow();
                            UpdateInfo(InfoType.RTWorking, 4);
                        }
                    }
                    else if ((InfoType)e.Result == InfoType.InvalidOS) { UpdateInfo((InfoType)e.Result, 0); }
                    else if ((InfoType)e.Result == InfoType.Error) { ReportError("RT Worker", ThreadException); }

                    TimeLabel.Text = "0h 0m 0s left";
                    ProcessCancelled = false;
                }
                else
                {
                    if ((InfoType)e.Result == InfoType.OK)
                    {
                        if (ProcessCancelled == true) { UpdateInfo(InfoType.RTWorking, 2); }
                        else
                        {
                            string SaveFileType = "*.jpg";
                            switch (MySettings.SavingFormat)
                            {
                                case 0:
                                    SaveFileType = "*.jpg";
                                    break;
                                case 1:
                                    SaveFileType = "*.png";
                                    break;
                                case 2:
                                    SaveFileType = "*.tiff";
                                    break;
                            }
                            string[] finishedFiles = Directory.GetFiles(MySettings.LastSaveDir, SaveFileType);

                            if (finishedFiles.Length != AllFiles.Count) { UpdateInfo(InfoType.RTWorking, 3); }
                            else { UpdateInfo(InfoType.RTWorking, 1); }
                        }
                    }
                    else if ((InfoType)e.Result == InfoType.InvalidOS) { UpdateInfo((InfoType)e.Result, 0); }
                    else if ((InfoType)e.Result == InfoType.Error) { ReportError("RT Worker", ThreadException); }

                    SaveButton.Sensitive = true;
                    FinishedRoutines((ComputerState)FinishedDoBox.Active);
                }

                deletePPFiles(MySettings.KeepPP);
                TimeLabel.Text = "0h 0m 0s left";
                ProcessCancelled = false;
                ProgressBar.Fraction = 0;
            }
            catch (Exception ex) { ReportError("RT Finished", ex); }
        });
    }

    private void ExiftoolBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                ProgressPulse.Stop();
                TimeLabel.Text = "0h 0m 0s left";
                ProgressBar.Fraction = 0;
                BackCounter.Stop();
                ProgressFileWatcher.EnableRaisingEvents = false;

                if ((InfoType)e.Result == InfoType.ProcessCancelled == true) { UpdateInfo(InfoType.ExiftoolWorking, 2); }
                else if ((InfoType)e.Result == InfoType.OK)
                {
                    for (int i = 0; i < AllFiles.Count; i++)
                    {
                        string imgname = System.IO.Path.GetFileNameWithoutExtension(AllFiles[i].Filename);
                        string savepath = Thumbpath + imgname + "_Thumb.jpg";

                        if (!File.Exists(savepath)) { UpdateInfo(InfoType.ExiftoolWorking, 3); break; }
                        else
                        {
                            Bitmap bmp = new Bitmap(savepath);
                            AllFiles[i].ThumbWidth = bmp.Width;
                            AllFiles[i].ThumbHeight = bmp.Height;
                            bmp.Dispose();
                        }
                    }

                    UpdateInfo(InfoType.ExiftoolWorking, 4);
                    UpdateTable();

                    //selecting the first image in table:
                    TreeIter fIter;
                    ValueTable.Model.GetIterFirst(out fIter);
                    ValueTable.SetCursor(ValueTable.Model.GetPath(fIter), ValueTable.Columns[0], false);
                }
                else if ((InfoType)e.Result == InfoType.Error) { ReportError("Exiftool Worker", ThreadException); }

                ProcessCancelled = false;
            }
            catch (Exception ex) { ReportError("Exiftool Finished", ex); }
        });
    }

    private void ThumbExtract_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            if ((InfoType)e.Result == InfoType.Error)
            {
                ProcessCancelled = true;

                object locker = new object();
                lock (locker)
                {
                    if (WorkerHasError == false)
                    {
                        WorkerHasError = true;
                        ReportError("Thumbnail Extract Worker", ThreadException);
                    }
                }
            }
        });
    }

    private void ExifCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            if ((InfoType)e.Result == InfoType.Error)
            {
                ReportError("Exif Calculation Worker", ThreadException);
                ProcessCancelled = true;
            }
        });
    }

    private void CalculationsBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
           {
               try
               {
                   BackCounter.Stop();

                   if ((InfoType)e.Result != InfoType.Error)
                   {
                       if ((InfoType)e.Result != InfoType.None)
                       {
                           if ((InfoType)e.Result == InfoType.Cancel || e.Cancelled == true || (InfoType)e.Result == InfoType.TooDark)
                           {
                               if ((InfoType)e.Result == InfoType.TooDark) { UpdateInfo(InfoType.TooDark, 0); }
                               else { UpdateInfo(InfoType.BrCalc, 4); }

                               for (int i = 0; i < CalcWorker.Count; i++)
                               {
                                   if (CalcWorker[i].IsBusy) { CalcWorker[i].CancelAsync(); }
                               }

                               UpdateTable();
                               FillCurveSelectBox();
                           }
                           else
                           {
                               double min = AllFiles.Min(p => p.Brightness);
                               if (min < 0)
                               {
                                   for (int i = 0; i < AllFiles.Count; i++)
                                   {
                                       AllFiles[i].Brightness += min + 5;
                                   }
                               }

                               List<double> Brtmp = new List<double>();
                               double maxchange = 0;
                               int maxindex = 1;
                               for (int i = 0; i < AllFiles.Count; i++)
                               {
                                   Brtmp.Add(AllFiles[i].Brightness);
                                   AllFiles[i].AltBrightness = AllFiles[i].Brightness;
                                   if (i > 0) { if (Math.Abs(AllFiles[i].Brightness - AllFiles[i - 1].Brightness) > maxchange) { maxindex = i - 1; } }
                               }

							PrevIndexSpin.Value = maxindex;
							PrevCountSpin.Value = 2;

                               AllCurves.InitBrCurves(Brtmp);
                               FillCurveSelectBox();
                               UpdateTable();
                               RefreshGraph(true);
                               BrightnessCalculated = true;
                               UpdateInfo(InfoType.BrCalc, 5);
                           }
                       }
                   }
                   else
                   {
                       BrightnessCalculated = false;
                       for (int i = 0; i < CalcWorker.Count; i++)
                       {
                           if (CalcWorker[i].IsBusy) { CalcWorker[i].CancelAsync(); }
                       }

                       object locker = new object();
                       lock (locker)
                       {
                           if (WorkerHasError == false)
                           {
                               WorkerHasError = true;
                               ReportError("Brightness Calculation", ThreadException);
                           }
                       }
                   }

                   TimeLabel.Text = "0h 0m 0s left";
                   ProgressBar.Fraction = 0;
               }
               catch (Exception ex) { ReportError("Calculations Finished", ex); }
           });
    }

    private void SaveBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            if ((bool)e.Result == true)
            {
                UpdateInfo(InfoType.Saving, 1);
                SetSaveStatus(true);
            }
            else { ReportError("Save Backgroundworker", ThreadException); }
        });
    }

    private void OpenBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                if ((bool)e.Result == true)
                {
                    checkPrograms();
                    RefreshGraph(true);
                    UpdateTable();
                    FillCurveSelectBox();

                    UpdateInfo(InfoType.Opening, 1);
                    SetSaveStatus(true);
                }
                else
                {
                    if (ThreadException == new FileNotFoundException()) { MessageBox.Show("Couldn´t find one or more images!" + Environment.NewLine + "(Searching for them will be implmented later)"); }
                    else { ReportError("Open Backgroundworker", ThreadException); }
                }
            }
            catch (Exception ex) { ReportError("Open Finished", ex); }
        });
    }

    private void XMPWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        try
        {
            if ((InfoType)e.Result == InfoType.OK)
            {
                UpdateInfo(InfoType.XMP, 0);
            }
            else if ((InfoType)e.Result == InfoType.Error)
            {
                ReportError("XMP Backgroundworker", ThreadException);
            }
        }
        catch (Exception ex) { ReportError("XMP Worker Finished", ex); }
    }

    #endregion Completed


    #region Progress

    private void ProgressFileWatcher_Created(object sender, FileSystemEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                ProgressPulse.Stop();

                if (RTBackground.IsBusy == true)
                {
                    int leftfiles;
                    if (ProgState == ProgramState.PreviewRender)
                    {
                        int filecount = Directory.GetFiles(System.IO.Path.GetDirectoryName(e.FullPath)).Length;
                        leftfiles = (int)PrevIndexSpin.Value - filecount;
                        ProgressBar.Fraction = (float)filecount / PrevIndexSpin.Value;
                    }
                    else
                    {
                        int filecount = Directory.GetFiles(System.IO.Path.GetDirectoryName(e.FullPath)).Length;
                        leftfiles = AllFiles.Count - filecount;
                        ProgressBar.Fraction = (float)filecount / (float)AllFiles.Count;
                    }

                    long passedtime = DateTime.Now.Ticks - lastTime;
                    lastTime = DateTime.Now.Ticks;
                    long lefttime = passedtime * leftfiles;
                    Elapsedtime = TimeSpan.FromTicks(lefttime);
                    TimeLabel.Text = Elapsedtime.Hours + "h " + Elapsedtime.Minutes + "m " + Elapsedtime.Seconds + "s left";
                }
                else if (ExiftoolBackground.IsBusy == true)
                {
                    if (ProgressBar.Fraction >= 1) { ProgressBar.Fraction = 0; }
                    int filecount = Directory.GetFiles(e.FullPath.Remove(e.FullPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar))).Length;
                    int leftfiles = AllFiles.Count - filecount;
                    if ((float)filecount / (float)AllFiles.Count <= 1f) { ProgressBar.Fraction = (float)filecount / (float)AllFiles.Count; }
                }
            }
            catch (Exception ex) { ReportError("Files Changed", ex); }
        });
    }

    //needs a bit of rewrite, not actual anymore
    private void CalculationsBackground_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                ProgressPulse.Stop();
                double progress = e.ProgressPercentage / 100f;

                if ((CalcState)e.UserState == CalcState.Mask)
                {
                    if (progress == 1)
                    {
                        TimeLabel.Text = "0h 0m 0s left";
                        ProgressBar.Fraction = 0;
                    }
                    if (progress <= 1 && progress >= 0) { ProgressBar.Fraction = progress; }
                    int currC = (int)(progress * (AllFiles.Count / MySettings.Threads));
                    int leftfiles = AllFiles.Count - currC;
                    long passedtime = DateTime.Now.Ticks - lastTime;
                    lastTime = DateTime.Now.Ticks;
                    long lefttime = passedtime * leftfiles;
                    Elapsedtime = TimeSpan.FromTicks(lefttime / 4);
                    TimeLabel.Text = Elapsedtime.Hours + "h " + Elapsedtime.Minutes + "m " + Elapsedtime.Seconds + "s left";
                }
                else if ((CalcState)e.UserState == CalcState.Brightness)
                {
                    if (progress == 0)
                    {
                        UpdateInfo(InfoType.BrCalc, 1);
                        BackCounter.Stop();
                        TimeLabel.Text = "0h 0m 0s left";
                        ProgressBar.Fraction = 0;
                    }

                    if (progress <= 1 && progress >= 0) { ProgressBar.Fraction = progress; }
                }

                #region not used atm

                /*else if ((CalcState)e.UserState == "PixelFind")
            {
                if (progress == 1)
                {
                    InfoLabel.Text = "Calculating (x/3)";
                    BackCounter.Enabled = true;
                    TimeLabel.Text = "0h 0m 0s left";
                    graph.Clear(System.Drawing.Color.DimGray);
                    CalcProgressBar.Value = 0;
                }

                CalcProgressBar.PerformStep();
                int leftfiles = allFiles.Count - progress;
                long passedtime = DateTime.Now.Ticks - lastTime;
                lastTime = DateTime.Now.Ticks;
                long lefttime = passedtime * leftfiles;
                Elapsedtime = TimeSpan.FromTicks(lefttime);
                TimeLabel.Text = Elapsedtime.Hours + "h " + Elapsedtime.Minutes + "m " + Elapsedtime.Seconds + "s left";
            }
            else if ((CalcState)e.UserState == "CalcPixBr")
            {
                if (progress == 0)
                {
                    InfoLabel.Text = "Calculating (x/3)";
                    TimeLabel.Text = "0h 0m 0s left";
                    CalcProgressBar.Value = 0;
                }

                CalcProgressBar.PerformStep();
            }*/

                #endregion not used atm

                else if ((CalcState)e.UserState == CalcState.CompareBrwithBv)
                {
                    if (progress == 0)
                    {
                        UpdateInfo(InfoType.BrCalc, 2);
                        TimeLabel.Text = "0h 0m 0s left";
                        ProgressBar.Fraction = 0;
                    }

                    if (progress <= 1 && progress >= 0) { ProgressBar.Fraction = progress; }
                }
                else if ((CalcState)e.UserState == CalcState.Statistics)
                {
                    if (progress == 0)
                    {
                        UpdateInfo(InfoType.BrCalc, 2);
                        TimeLabel.Text = "0h 0m 0s left";
                        ProgressBar.Fraction = 0;
                    }

                    if (progress <= 1 && progress >= 0) { ProgressBar.Fraction = progress; }
                }
            }
            catch (Exception ex)
            {
                UpdateInfo(InfoType.BrCalc, 3);
                ReportError("Calculations Changed", ex);
            }
        });
    }

    private void ExiftoolBackground_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            try { if ((string)e.UserState == "ChangeState") { UpdateInfo(InfoType.ExiftoolWorking, 1); ProgressPulse.Start(); } }
            catch (Exception ex) { ReportError("Exiftool Changed", ex); }
        });
    }

    private void General_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
            {
                ProgressPulse.Stop();
                double progress = e.ProgressPercentage / 100;
                ProgressBar.Fraction = progress;
            });
    }

    private bool BackCounter_Tick()
    {
        if (BackCounter.Enabled)
        {
            if (Elapsedtime.TotalSeconds > 0)
            {
                Elapsedtime = TimeSpan.FromSeconds(Elapsedtime.TotalSeconds - 1);
                Gtk.Application.Invoke(delegate { TimeLabel.Text = Elapsedtime.Hours + "h " + Elapsedtime.Minutes + "m " + Elapsedtime.Seconds + "s left"; });
            }
        }
        return BackCounter.Enabled;
    }

    #endregion Progress

}