using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Linq;
using Cairo;
using DeSERt;
using Gdk;
using Gtk;

public partial class DeSERtMain : Gtk.Window
{

    #region Form

    public DeSERtMain() : base(Gtk.WindowType.Toplevel)
    {
        try
        {
            Build();
            MySettings.Load();
            InitForm();
        }
        catch (Exception ex) { ReportError("Init Form", ex); }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        a.RetVal = Quit(ClosingReason.User);
    }

    protected void OnExposeEvent(object o, ExposeEventArgs args)
    {
        Gtk.Application.Invoke(delegate { RefreshGraph(false); });
    }

    protected void OnGraphEventsButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        try
        {
            //left button = 1
            if (args.Event.Button == 1)
            {
                double x = args.Event.X;
                double y = args.Event.Y;
                GCurves.GCurve tmp;
                AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);
                List<PointF> tmpGPoints = new List<PointF>();
                for (int i = 0; i < tmp.Points.Count; i++)
                {
                    tmpGPoints.Add(GetGraphXY(tmp.Points[i].Value));
                }

                for (int i = 0; i < tmp.Points.Count; i++)
                {
                    if (x > tmpGPoints[i].X - 5 && x < tmpGPoints[i].X + 5 && y > tmpGPoints[i].Y - 5 && y < tmpGPoints[i].Y + 5 && !tmp.Points[i].Key)
                    {
                        //hit point:
                        MovePoint = new KeyValuePair<int, bool>(i, true);
                        tmp.SelectedPoint = i;
                        //Fill the Valueboxes with the current value
                        XValBox.Text = (tmp.Points[tmp.SelectedPoint].Value.X + 1).ToString();
                        YValBox.Text = tmp.Points[tmp.SelectedPoint].Value.Y.ToString();
                        RefreshGraph(false);
                        return;
                    }
                    else if (x > tmpGPoints[i].X - 5 && x < tmpGPoints[i].X + 5 && tmp.Points[i].Key) { return; }
                }

                if (x > GrBorLeft + 3 && x < tmpGPoints[tmpGPoints.Count - 1].X - 3 && y < GraphArea.Allocation.Height - GrBor && y > GrBor)
                {
                    tmp.AddPoint(GetValueFromXY((float)x, GraphArea.Allocation.Height - (float)y));
                    MovePoint = new KeyValuePair<int, bool>(tmp.SelectedPoint, true);
                    RefreshGraph(false);
                }
            }
        }
        catch (Exception ex) { ReportError("Graph Mouse Down", ex); }
    }

    protected void OnGraphEventsButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
    {
        try
        {
            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            double x = args.Event.X;
            double y = args.Event.Y;
            if (MovePoint.Value == true)
            {
                ProjectSaved = false;
                if (y < 0 || GraphArea.Allocation.Height - y < GrBor || x < GrBor || x > GraphArea.Allocation.Width)
                {
                    if (MovePoint.Key != 0 && MovePoint.Key != tmp.Points.Count - 1)
                    {
                        tmp.RemovePoint(MovePoint.Key);
                    }
                    RefreshGraph(false);
                }
            }

            MovePoint = new KeyValuePair<int, bool>(MovePoint.Key, false);
            
            //Fill the Valueboxes with the current value
            XValBox.Text = (tmp.Points[tmp.SelectedPoint].Value.X + 1).ToString();
            YValBox.Text = tmp.Points[tmp.SelectedPoint].Value.Y.ToString();
        }
        catch (Exception ex) { ReportError("Graph Mouse Up", ex); }
    }

    protected void OnMouseMoveEvent(object o, MotionNotifyEventArgs args)
    {
        try
        {
            if (MovePoint.Value == true)
            {
                GCurves.GCurve tmp;
                AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

                float x = (float)args.Event.X;
                float y = (float)args.Event.Y;
                ProjectSaved = false;
                List<PointF> tmpGPoints = new List<PointF>();
                for (int i = 0; i < tmp.Points.Count; i++)
                {
                    tmpGPoints.Add(GetGraphXY(tmp.Points[i].Value));
                }

                if (MovePoint.Key > 0 && MovePoint.Key < tmp.Points.Count - 1)
                {
                    if (tmpGPoints[MovePoint.Key - 1].X + 3 < x && tmpGPoints[MovePoint.Key + 1].X - 3 > x)
                    {
                        tmp.Points[MovePoint.Key] = new KeyValuePair<bool,PointF>(tmp.Points[MovePoint.Key].Key, GetValueFromXY(x, GraphArea.Allocation.Height - y));
                        RefreshGraph(false);
                    }
                }
                else if (MovePoint.Key == 0 || MovePoint.Key == tmp.Points.Count - 1)
                {
                    if (GraphArea.Allocation.Height - y < GraphArea.Allocation.Height - GrBor && GraphArea.Allocation.Height - y > GrBor)
                    {
                        double oldX = tmpGPoints[MovePoint.Key].X;
                        tmp.Points[MovePoint.Key] = new KeyValuePair<bool, PointF>(tmp.Points[MovePoint.Key].Key, GetValueFromXY((float)oldX, GraphArea.Allocation.Height - y));
                        RefreshGraph(false);
                    }
                }
                
                //Fill the Valueboxes with the current value
                XValBox.Text = (tmp.Points[tmp.SelectedPoint].Value.X + 1).ToString();
                YValBox.Text = tmp.Points[tmp.SelectedPoint].Value.Y.ToString();
            }
        }
        catch (Exception ex) { ReportError("Graph Mouse Move", ex); }
    }

    #endregion Form


    #region Buttons

    protected void OnProcOpenButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (!IsBusy() && AllFiles.Count > 1)
            {
                FileChooserDialog fc = new FileChooserDialog("Open PP3", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
                if (Directory.Exists(MySettings.LastPPDir)) { fc.SetCurrentFolder(MySettings.LastPPDir); }

                FileFilter filter = new FileFilter();
                filter.Name = "Postprocessing Profile";
                filter.AddPattern("*.pp3");
                fc.AddFilter(filter);

                if (fc.Run() == (int)ResponseType.Accept)
                {
                    MySettings.LastPPDir = System.IO.Path.GetDirectoryName(fc.Filename);
                    MySettings.Save();
                    SetSaveStatus(false);

                    MainPP3.ReadFile(fc.Filename);
                    MainPP3.Name = "Main";
                    PPfileOpened = true;

                    if (AllFiles.Count > 0) { AllFiles[0].IsKeyframe = true; }

                    for (int i = 0; i < AllFiles.Count; i++) { if (!AllFiles[i].IsKeyframe || i == 0) { AllFiles[i].PP3 = new PP3Values(fc.Filename); AllFiles[i].PP3.Name = "Main"; } }
                    AllFiles[0].IsKeyframe = true;

                    AllCurves.InitPP3Curves(MainPP3, AllFiles.Count);
                    UpdateTable();
                    FillCurveSelectBox();
                    UpdateInfo(InfoType.PPFile, 1);
                }
                fc.Destroy();
            }
            else if (AllFiles.Count < 2) { UpdateInfo(InfoType.Imagecount, 0); }
        }
        catch (Exception ex) { ReportError("Browse PP File", ex); PPfileOpened = false; }
    }

    protected void OnImageOpenButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (!IsBusy() && checkPrograms())
            {
                FileChooserDialog fc = new FileChooserDialog("Open Images", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
                if (Directory.Exists(MySettings.LastPicDir)) { fc.SetCurrentFolder(MySettings.LastPicDir); }

                if (fc.Run() == (int)ResponseType.Accept)
                {
                    #region Get all supported images

                    List<string> Files = new List<string>();
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.jpg", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.jpeg", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.CR2", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.CRW", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.png", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.tif", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.tiff", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.x3f", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.NEF", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.srw", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.srf", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.sr2", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.arw", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.erf", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.pef", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.raf", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.3fr", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.fff", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.dcr", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.dcs", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.kdc", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.rwl", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.mrw", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.mdc", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.nrw", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.orf", SearchOption.TopDirectoryOnly));
                    Files.AddRange(Directory.GetFiles(fc.Filename, "*.rw2", SearchOption.TopDirectoryOnly));

                    #endregion

                    if (Files.Count > 1)
                    {
                        AllFiles.Clear();
                        if (BrightnessCalculated)
                        {
                            AllCurves.BrInCurve = null;
                            BrightnessCalculated = false;
                            GCurves.GCurve tmp;
                            AllCurves.Curves.TryGetValue(GCurves.CurveName.Exposure_Compensation.ToString(), out tmp);
                            tmp.ClearPoints();
                        }
                        deleteThumbs();

                        Workpath = fc.Filename;
                        MySettings.LastPicDir = fc.Filename;
                        MySettings.Save();

                        for (int i = 0; i < Files.Count; i++)
                        {
                            AllFiles.Add(new FileData(Files[i]));
                            if (PPfileOpened) { AllFiles[i].PP3 = new PP3Values(MainPP3.Path); }
                            else { AllFiles[i].PP3 = new PP3Values(); }
                        }

                        if (PPfileOpened) { AllFiles[0].IsKeyframe = true; }

                        PrevIndexSpin.Adjustment.Upper = AllFiles.Count - 1;
                        PrevCountSpin.Adjustment.Upper = AllFiles.Count - PrevIndexSpin.Value + 1;

                        UpdateTable();
                        GetExifData();

                        SetSaveStatus(false);
                    }
                    else { UpdateInfo(InfoType.Imagecount, 0); }
                }
                fc.Destroy();
            }
        }
        catch (Exception ex) { ReportError("Browse Pictures", ex); BrightnessCalculated = false; }
    }

    protected void OnCalculateButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (!IsBusy() && AllFiles.Count > 1 && !BrightnessCalculated)
            {
                MainNotebook.CurrentPage = 1;
                UpdateInfo(InfoType.BrCalc, 0);
                SetSaveStatus(false);
                StartCalcThreads();
            }
        }
        catch (Exception ex) { ReportError("Calculate Button", ex); }
    }

    protected void OnSaveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (IsSafe() && !IsBusy())
            {
                UpdateInfo(InfoType.Wait, 0);
                ResponseType result = ResponseType.Yes;

                if (HitRoof()) { result = UpdateInfo(InfoType.HitRoof, 0); }

                if (result == ResponseType.Yes)
                {
                    FileChooserDialog fc = new FileChooserDialog("Save Images", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);
                    
                    if (Directory.Exists(MySettings.LastSaveDir)) { fc.SetCurrentFolder(MySettings.LastSaveDir); }

                    if (fc.Run() == (int)ResponseType.Accept && checkPrograms())
                    {
                        string[] files = Directory.GetFiles(fc.Filename);
                        if (files.Length > 0)
                        {
                            if (UpdateInfo(InfoType.FolderExists, 0) == ResponseType.Yes)
                            {
                                if (MessageBox.Show("Are you sure you want to delete all Files in this directory?" + Environment.NewLine + "Those files will NOT be moved to the recycle bin!", MessageType.Warning, ButtonsType.YesNo) == ResponseType.Yes)
                                {
                                    bool error = false;
                                    foreach (string path in files)
                                    {
                                        int count = 0;
                                        while (count < 5)
                                        {
                                            try { File.Delete(path); break; }
                                            catch { count++; Thread.Sleep(10); }
                                        }
                                        if (count == 5) { error = true; }
                                    }
                                    if (error) { UpdateInfo(InfoType.FileRemoveError, 0); fc.Destroy(); return; }
                                }
                            }
                            else { fc.Destroy(); return; }
                        }

                        MySettings.LastSaveDir = fc.Filename;
                        MySettings.Save();
                        SaveButton.Sensitive = false;
                        if (InterpolatePP3())
                        {
                            if (MySettings.Program == 0) { writePPFiles(); StartRT(fc.Filename); }
                            else if (MySettings.Program == 1) { writeXMPFiles(); StartLR(); }
                        }
                        else { fc.Destroy(); return; }
                    }
                    fc.Destroy();
                }
                else { UpdateInfo(InfoType.Cancel, 0); }
            }
        }
        catch (Exception ex) { ReportError("Save Button", ex); }
    }

    protected void OnCancelButtonClicked(object sender, EventArgs e)
    {
        try
        {
            KillRunningProcess();
            ProgressPulse.Stop();
            ProgressBar.Fraction = 0;
        }
        catch (Exception ex) { ReportError("Cancel Button", ex); }
    }

    protected void OnEndToStartButtonClicked(object sender, EventArgs e)
    {
        try
        {
            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            if (tmp.Points.Count > 1)
            {
                tmp.Points[tmp.Points.Count - 1] = new KeyValuePair<bool,PointF>(tmp.Points[tmp.Points.Count - 1].Key, new PointF(tmp.Points[tmp.Points.Count - 1].Value.X, tmp.Points[0].Value.Y));
                RefreshGraph(false);
                SetSaveStatus(false);
            }
        }
        catch (Exception ex) { ReportError("End To Start Button", ex); }
    }

    protected void OnStartToEndButtonClicked(object sender, EventArgs e)
    {
        try
        {
            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            if (tmp.Points.Count > 1)
            {
                tmp.Points[0] = new KeyValuePair<bool,PointF>(tmp.Points[0].Key, new PointF(tmp.Points[0].Value.X, tmp.Points[tmp.Points.Count - 1].Value.Y));
                RefreshGraph(false);
                SetSaveStatus(false);
            }
        }
        catch (Exception ex) { ReportError("Start To End Button", ex); }
    }

    protected void OnResetCurveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            if (tmp.Points.Count > 1 && !IsBusy())
            {
                BrScale.Value = 100;

                tmp.ResetCurve(AllFiles.Count);

                MovePoint = new KeyValuePair<int, bool>(0, false);
                tmp.SelectedPoint = 0;

                RefreshGraph(false);
                SetSaveStatus(false);
            }
        }
        catch (Exception ex) { ReportError("Reset Curve Button", ex); }
    }

	protected void OnPreviewRenderButtonClicked (object sender, EventArgs e)
    {
        try { RenderPreview(); }
        catch (Exception ex) { ReportError("Preview Render Button", ex); }
	}

    #endregion Buttons


    #region Other Elements

    protected void OnCurveSelectBoxChanged(object sender, EventArgs e)
    {
        if (CurveSelectBox.Active != -1)
        {
            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            int pointcount = 0;
            if (tmp != null) { pointcount = tmp.Points.Count; }

            if (pointcount > 0)
            {
                //Fill the Valueboxes with the current value
                XValBox.Text = tmp.Points[tmp.SelectedPoint].Value.X.ToString();
                YValBox.Text = tmp.Points[tmp.SelectedPoint].Value.Y.ToString();
            }
            RefreshGraph(true);
        }
    }

    protected void OnXValBoxTextInserted(object o, TextInsertedArgs args)
    {
        try { UpdateValueBox(); }
        catch (Exception ex) { ReportError("Valuebox", ex); }
    }

    protected void OnYValBoxTextInserted(object o, TextInsertedArgs args)
    {
        try { UpdateValueBox(); }
        catch (Exception ex) { ReportError("Valuebox", ex); }
    }

    protected void OnValueTableCursorChanged(object sender, EventArgs e)
    {
        try
        {
            if (AllFiles.Count > 0 && !IsBusy())
            {   
                if (ValueTable.Selection.CountSelectedRows() == 1)
                {
                    TreeIter iter;
                    table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                    int index = table.GetPath(iter).Indices[0];
                    string path = System.IO.Path.Combine(Thumbpath, System.IO.Path.GetFileNameWithoutExtension(AllFiles[index].Filename) + "_Thumb.jpg");
                    Pixbuf img;

                    try { img = new Pixbuf(path); }
                    catch { UpdateInfo(InfoType.OpenImgError, 0); return; }

                    PreviewImg.Pixbuf = img;
                }
            }
        }
        catch (Exception ex) { ReportError("Table Selection", ex); }
    }

    protected void CellEdited(object o, EditedArgs args)
    {        
        try
        {
            if (BrightnessCalculated)
            {
                double val;
                try { val = Convert.ToDouble(args.NewText); }
                catch { return; }

                TreeIter iter;
                table.GetIter(out iter, new TreePath(args.Path));
                int index = table.GetPath(iter).Indices[0];

                double change = val - AllFiles[index].AltBrightness;
                AllFiles[index].AltBrightness = val;

                for (int i = index + 1; i < AllFiles.Count; i++)
                {
                    AllFiles[i].AltBrightness += change;
                }

                double min = AllFiles.Min(p => p.AltBrightness);
                if (min < 0)
                {
                    for (int i = 0; i < AllFiles.Count; i++)
                    {
                        AllFiles[i].AltBrightness += min + 5;
                    }
                    change += min + 5;
                }
                
                List<double> tmpbr = new List<double>();
                for (int i = 0; i < AllFiles.Count; i++) { tmpbr.Add(AllFiles[i].AltBrightness); }
                AllCurves.UpdateBrCurve(tmpbr, change, index);
                UpdateTable();
            }
        }
        catch (Exception ex) { ReportError("Brightness Cell Edited", ex); }
    }

	protected void OnBrScaleValueChanged (object sender, EventArgs e)
	{
        try
        {
            if (BrightnessCalculated)
            {
                BrScaleEntry.Text = BrScale.Value.ToString("N2");

                double oldBr = AllFiles[0].AltBrightness;
                double[] change = new double[AllFiles.Count];

                for (int i = 1; i < AllFiles.Count; i++)
                {
                    change[i] = (AllFiles[i].Brightness - AllFiles[i-1].Brightness) * BrScale.Value / 100;
                }

                for (int i = 1; i < AllFiles.Count; i++)
                {
                    AllFiles[i].AltBrightness = AllFiles[i - 1].AltBrightness + change[i];
                }
                
                double min = AllFiles.Min(p => p.AltBrightness);
                if (min <= 0)
                {
                    for (int i = 0; i < AllFiles.Count; i++)
                    {
                        AllFiles[i].AltBrightness += min + 5;
                    }
                }

                double BrCh = AllFiles[0].AltBrightness - oldBr;

                List<double> tmpbr = new List<double>();
                for (int i = 0; i < AllFiles.Count; i++) { tmpbr.Add(AllFiles[i].AltBrightness); }
                AllCurves.UpdateBrCurve(tmpbr, BrCh, 0);
                RefreshGraph(true);
                UpdateTable();
            }
        }
        catch (Exception ex) { ReportError("Brightness Slider", ex); }
	}
	
    [GLib.ConnectBefore]
	protected void OnBrScaleEntryKeyPressEvent (object o, KeyPressEventArgs args)
	{
        try
        {
            if (BrightnessCalculated)
            {
                if (args.Event.Key == Gdk.Key.Return)
                {
                    double newval = 0;
                    try { newval = Convert.ToDouble(BrScaleEntry.Text.Replace('.', ',')); }
                    catch { UpdateInfo(InfoType.InvalidNumber, 0); return; }

                    if (newval > BrScale.Adjustment.Upper || newval < BrScale.Adjustment.Lower) { UpdateInfo(InfoType.HighLowValue, 0); return; }
                    else { UpdateInfo(InfoType.Default, 0); }

                    BrScale.Value = newval;
                }
            }
            else { BrScaleEntry.Text = "100,00"; }
        }
        catch (Exception ex) { ReportError("Brightness Scale Entry", ex); }
	}

    protected void OnPrevIndexSpinChanged(object sender, EventArgs e)
    {
        PrevCountSpin.Adjustment.Upper = AllFiles.Count - PrevIndexSpin.Value + 1;
    }

    #endregion Other Elements


    #region Context Menu

    [GLib.ConnectBefore]
    protected void OnValueTableButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        if (args.Event.Button == 3)
        {
            Menu MyContextMenu = new Menu();
            Menu SubFilterset = new Menu();
            Menu SubAddFilterset = new Menu();
            Menu SubRemFilterset = new Menu();
            Menu SubDarkframe = new Menu();
            Menu SubAddDarkframe = new Menu();
            Menu SubRemDarkframe = new Menu();
            Menu SubKeyframe = new Menu();
            Menu SubBrigthness = new Menu();
            
            #region Keyframe

            MenuItem KeyframeMenu = new MenuItem("Keyframe");
            MyContextMenu.Add(KeyframeMenu);
            KeyframeMenu.Submenu = SubKeyframe;

            MenuItem OpenKeyframe = new MenuItem("Add");
            OpenKeyframe.ButtonPressEvent += AddKeyframe_ButtonPressEvent;
            SubKeyframe.Add(OpenKeyframe);

            MenuItem RemoveKeyframe = new MenuItem("Remove");
            RemoveKeyframe.ButtonPressEvent += RemoveKeyframe_ButtonPressEvent;
            SubKeyframe.Add(RemoveKeyframe);

            #endregion

            #region Filterset

            MenuItem FiltersetMenu = new MenuItem("Filterset");
            MyContextMenu.Add(FiltersetMenu);
            FiltersetMenu.Submenu = SubFilterset;

            MenuItem SubAddFiltersetMenu = new MenuItem("Open");
            SubFilterset.Add(SubAddFiltersetMenu);
            SubAddFiltersetMenu.Submenu = SubAddFilterset;

            MenuItem SingleFilterItemOpen = new MenuItem("Single Image");
            SingleFilterItemOpen.ButtonPressEvent += SingleFilterItemOpen_ButtonPressEvent;
            SubAddFilterset.Add(SingleFilterItemOpen);
            MenuItem TillEndFilterItemOpen = new MenuItem("Till End");
            TillEndFilterItemOpen.ButtonPressEvent += FollowingFilterItemOpen_ButtonPressEvent;
            SubAddFilterset.Add(TillEndFilterItemOpen);
            MenuItem TillNextFilterItemOpen = new MenuItem("Till Next Filter");
            TillNextFilterItemOpen.ButtonPressEvent += TillNextFilterItemOpen_ButtonPressEvent;
            SubAddFilterset.Add(TillNextFilterItemOpen);
            
            MenuItem SubRemFiltersetMenu = new MenuItem("Remove");
            SubFilterset.Add(SubRemFiltersetMenu);
            SubRemFiltersetMenu.Submenu = SubRemFilterset;

            MenuItem SingleFilterItemRem = new MenuItem("Single Image");
            SingleFilterItemRem.ButtonPressEvent += SingleFilterItemRemove_ButtonPressEvent;
            SubRemFilterset.Add(SingleFilterItemRem);
            MenuItem TillEndFilterItemRem = new MenuItem("Till End");
            TillEndFilterItemRem.ButtonPressEvent += FollowingFilterItemRemove_ButtonPressEvent;
            SubRemFilterset.Add(TillEndFilterItemRem);
            MenuItem TillNextFilterItemRem = new MenuItem("Till Next Filter");
            TillNextFilterItemRem.ButtonPressEvent += TillNextFilterItemRemove_ButtonPressEvent;
            SubRemFilterset.Add(TillNextFilterItemRem);

            #endregion

            #region Darkframe
            
            MenuItem DarkframeMenu = new MenuItem("Darkframe");
            MyContextMenu.Add(DarkframeMenu);
            DarkframeMenu.Submenu = SubDarkframe;

            MenuItem OpenDarkframe = new MenuItem("Open");
            SubDarkframe.Add(OpenDarkframe);
            OpenDarkframe.Submenu = SubAddDarkframe;

            MenuItem SingleDarkframeOpen = new MenuItem("Single Image");
            SingleDarkframeOpen.ButtonPressEvent += SingleDarkframeOpen_ButtonPressEvent;
            SubAddDarkframe.Add(SingleDarkframeOpen);
            MenuItem TillEndDarkframeOpen = new MenuItem("Till End");
            TillEndDarkframeOpen.ButtonPressEvent += FollowingDarkframeOpen_ButtonPressEvent;
            SubAddDarkframe.Add(TillEndDarkframeOpen);
            MenuItem TillNextDarkframeOpen = new MenuItem("Till Next Darkframe");
            TillNextDarkframeOpen.ButtonPressEvent += TillNextDarkframeOpen_ButtonPressEvent;
            SubAddDarkframe.Add(TillNextDarkframeOpen);

            MenuItem RemoveDarkframe = new MenuItem("Remove");
            SubDarkframe.Add(RemoveDarkframe);
            RemoveDarkframe.Submenu = SubRemDarkframe;

            MenuItem SingleDarkframeRem = new MenuItem("Single Image");
            SingleDarkframeRem.ButtonPressEvent += SingleDarkframeRemove_ButtonPressEvent;
            SubRemDarkframe.Add(SingleDarkframeRem);
            MenuItem TillEndDarkframeRem = new MenuItem("Till End");
            TillEndDarkframeRem.ButtonPressEvent += FollowingDarkframeRemove_ButtonPressEvent;
            SubRemDarkframe.Add(TillEndDarkframeRem);
            MenuItem TillNextDarkframeRem = new MenuItem("Till Next Darkframe");
            TillNextDarkframeRem.ButtonPressEvent += TillNextDarkframeRemove_ButtonPressEvent;
            SubRemDarkframe.Add(TillNextDarkframeRem);

            #endregion            

            #region Brightness

            MenuItem BrightnessMenu = new MenuItem("Brightness");
            MyContextMenu.Add(BrightnessMenu);
            BrightnessMenu.Submenu = SubBrigthness;

            MenuItem ResetBrSingle = new MenuItem("Reset Brightness");
            ResetBrSingle.ButtonPressEvent += ResetBrSingle_ButtonPressEvent;
            SubBrigthness.Add(ResetBrSingle);

            MenuItem ResetBrComplete = new MenuItem("Reset All Brightness");
            ResetBrComplete.ButtonPressEvent += ResetBrComplete_ButtonPressEvent;
            SubBrigthness.Add(ResetBrComplete);

            #endregion

            MyContextMenu.ShowAll();
            MyContextMenu.Popup();
        }
    }

    
    protected void SingleFilterItemOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenFilter(TableChangeType.Single);
    }

    protected void FollowingFilterItemOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenFilter(TableChangeType.TillEnd);
    }

    protected void TillNextFilterItemOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenFilter(TableChangeType.TillNext);
    }

    protected void SingleFilterItemRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveFilter(TableChangeType.Single);
    }

    protected void FollowingFilterItemRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveFilter(TableChangeType.TillEnd);
    }

    protected void TillNextFilterItemRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveFilter(TableChangeType.TillNext);
    }


    protected void SingleDarkframeOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenDarkframe(TableChangeType.Single);
    }

    protected void FollowingDarkframeOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenDarkframe(TableChangeType.TillEnd);
    }

    protected void TillNextDarkframeOpen_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        OpenDarkframe(TableChangeType.TillNext);
    }

    protected void SingleDarkframeRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveDarkframe(TableChangeType.Single);
    }

    protected void FollowingDarkframeRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveDarkframe(TableChangeType.TillEnd);
    }

    protected void TillNextDarkframeRemove_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveDarkframe(TableChangeType.TillNext);
    }


    protected void AddKeyframe_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        AddKeyframe();
    }

    protected void RemoveKeyframe_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        RemoveKeyframe();
    }


    protected void ResetBrSingle_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        ResetTableBrightness(TableChangeType.Single);
    }

    protected void ResetBrComplete_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
        ResetTableBrightness(TableChangeType.All);
    }

    #endregion Context Menu


    #region Menu

    protected void OnNewActionActivated(object sender, EventArgs e)
    {
        try { if (!IsBusy()) { NewProject(); } }
        catch (Exception ex) { ReportError("New Project", ex); }
    }

    protected void OnSaveActionActivated(object sender, EventArgs e)
    {
        try { if (!IsBusy()) { StartSaveProjectWorker(false); } }
        catch (Exception ex) { ReportError("Save Project", ex); }
    }

    protected void OnSaveAsActionActivated(object sender, EventArgs e)
    {
        try { if (!IsBusy()) { StartSaveProjectWorker(true); } }
        catch (Exception ex) { ReportError("Save Project As", ex); }
    }

    protected void OnOpenActionActivated(object sender, EventArgs e)
    {
        try { if (!IsBusy()) { StartOpenProjectWorker(); } }
        catch (Exception ex) { ReportError("Open Project", ex); }
    }

    protected void OnPreferencesActionActivated(object sender, EventArgs e)
    {
        DeSERt.Settings sWindow = new DeSERt.Settings();
        sWindow.ShowNow();
    }

    protected void OnHelpActionActivated(object sender, EventArgs e)
    {
        Help hWindow = new Help();
        hWindow.Show();
    }

    protected void OnAboutActionActivated(object sender, EventArgs e)
    {
        MyAbout aWindow = new MyAbout();
        aWindow.ShowNow();
    }
	
	protected void OnCreateFilterActionActivated(object sender, EventArgs e)
	{
		//CreateFilterset cWindow = new CreateFilterset();
		//cWindow.Show();
        MessageBox.Show("This feature is not finished yet. Sorry!","Not Finished!", MessageType.Info, ButtonsType.Ok);
	}

    protected void OnQuitActionActivated(object sender, EventArgs e)
    {
        Quit(ClosingReason.User);
    }

    #endregion Menu

}