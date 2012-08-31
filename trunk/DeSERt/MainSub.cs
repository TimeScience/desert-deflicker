using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Cairo;
using DeSERt;
using Gdk;
using Gtk;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Runtime.Serialization.Formatters.Binary;

public partial class DeSERtMain
{

    #region GUI Subroutines

    private void InitForm()
    {
        MainPP3 = new PP3Values();
        MainPP3.Name = "Main";
        ProgressPulse = new PulseBar(ProgressBar);
        deleteThumbs();
        InitGraph();
        InitTable();
        InitBackgroundWorker();
        checkPrograms();
        SetSaveStatus(true);
    }

    private void InitGraph()
    {
        //this is necessary before doing init of the Graph or otherwise Gtk# crashes:
        MainNotebook.CurrentPage = 1;

        AllCurves = new GCurves();

        GraphEvents.AddEvents((int)EventMask.PointerMotionMask);
        GraphEvents.MotionNotifyEvent += OnMouseMoveEvent;

        Graph = CairoHelper.Create(GraphArea.GdkWindow);
        Graph.Antialias = Antialias.Subpixel;
        GraphArea.DoubleBuffered = false;

        MainGraph = new ImageSurface(Format.Argb32, GraphArea.Allocation.Width, GraphArea.Allocation.Height);
        CurveGraph = new ImageSurface(Format.Argb32, GraphArea.Allocation.Width, GraphArea.Allocation.Height);
        ScaleGraph = new ImageSurface(Format.Argb32, GraphArea.Allocation.Width, GraphArea.Allocation.Height);        

        MainNotebook.CurrentPage = 0;
    }
    
    private void InitTable()
    {
        TreeViewColumn NrColumn = new TreeViewColumn();
        TreeViewColumn FileColumn = new TreeViewColumn();
        TreeViewColumn BrightnessColumn = new TreeViewColumn();
        TreeViewColumn AVColumn = new TreeViewColumn();
        TreeViewColumn TVColumn = new TreeViewColumn();
        TreeViewColumn ISOColumn = new TreeViewColumn();
        TreeViewColumn BVColumn = new TreeViewColumn();
        TreeViewColumn DarkframeColumn = new TreeViewColumn();
        TreeViewColumn FilterColumn = new TreeViewColumn();
        TreeViewColumn KeyframeColumn = new TreeViewColumn();

        CellRendererText NrCell = new CellRendererText();
        CellRendererText FileCell = new CellRendererText();
        CellRendererText BrightnessCell = new CellRendererText();
        CellRendererText AVCell = new CellRendererText();
        CellRendererText TVCell = new CellRendererText();
        CellRendererText ISOCell = new CellRendererText();
        CellRendererText BVCell = new CellRendererText();
        CellRendererText DarkframeCell = new CellRendererText();
        CellRendererText FilterCell = new CellRendererText();
        CellRendererText KeyframeCell = new CellRendererText();
        
        NrColumn.Title = "Nr";
        NrColumn.MinWidth = 35;
        NrColumn.PackStart(NrCell, true);

        FileColumn.Title = "Filename";
        FileColumn.MinWidth = 100;
        FileColumn.PackStart(FileCell, true);

        BrightnessColumn.Title = "Brightness";
        BrightnessColumn.MinWidth = 70;
        BrightnessColumn.PackStart(BrightnessCell, true);
        BrightnessCell.Editable = true;
        BrightnessCell.Edited += CellEdited;
        
        AVColumn.Title = "AV";
        AVColumn.MinWidth = 40;
        AVColumn.PackStart(AVCell, true);

        TVColumn.Title = "TV";
        TVColumn.MinWidth = 40;
        TVColumn.PackStart(TVCell, true);

        ISOColumn.Title = "ISO";
        ISOColumn.MinWidth = 40;
        ISOColumn.PackStart(ISOCell, true);

        BVColumn.Title = "BV";
        BVColumn.MinWidth = 40;
        BVColumn.PackStart(BVCell, true);
        
        DarkframeColumn.Title = "Darkframes";
        DarkframeColumn.MinWidth = 90;
        DarkframeColumn.PackStart(DarkframeCell, true);

        FilterColumn.Title = "Filtersets";
        FilterColumn.MinWidth = 90;
        FilterColumn.PackStart(FilterCell, true);

        KeyframeColumn.Title = "Keyframes";
        KeyframeColumn.MinWidth = 90;
        KeyframeColumn.PackStart(KeyframeCell, true);

        ValueTable.AppendColumn(NrColumn);
        ValueTable.AppendColumn(FileColumn);
        ValueTable.AppendColumn(BrightnessColumn);
        ValueTable.AppendColumn(AVColumn);
        ValueTable.AppendColumn(TVColumn);
        ValueTable.AppendColumn(ISOColumn);
        ValueTable.AppendColumn(BVColumn);
        ValueTable.AppendColumn(DarkframeColumn);
        ValueTable.AppendColumn(FilterColumn);
        ValueTable.AppendColumn(KeyframeColumn);

        NrColumn.AddAttribute(NrCell, "text", (int)TableLocation.Nr);
        FileColumn.AddAttribute(FileCell, "text", (int)TableLocation.Filename);
        BrightnessColumn.AddAttribute(BrightnessCell, "text", (int)TableLocation.Brightness);
        AVColumn.AddAttribute(AVCell, "text", (int)TableLocation.AV);
        TVColumn.AddAttribute(TVCell, "text", (int)TableLocation.TV);
        ISOColumn.AddAttribute(ISOCell, "text", (int)TableLocation.ISO);
        BVColumn.AddAttribute(BVCell, "text", (int)TableLocation.BV);
        DarkframeColumn.AddAttribute(DarkframeCell, "text", (int)TableLocation.Darkframes);
        FilterColumn.AddAttribute(FilterCell, "text", (int)TableLocation.Filtersets);
        KeyframeColumn.AddAttribute(KeyframeCell, "text", (int)TableLocation.Keyframes);

        ValueTable.Model = table;

        ValueTable.ButtonPressEvent += OnValueTableButtonPressEvent;
    }

    private void InitBackgroundWorker()
    {
        ExiftoolBackground = new BackgroundWorker();
        RTBackground = new BackgroundWorker();
        SaveBackground = new BackgroundWorker();
        OpenBackground = new BackgroundWorker();
        BackCounter = new MyTimer(new GLib.TimeoutHandler(BackCounter_Tick));
        CalcWorker = new List<BackgroundWorker>();
        ExifWorker = new BackgroundWorker();

        BackCounter.Time = 1000;

        ProgressFileWatcher = new FileSystemWatcher();
        ProgressFileWatcher.Created += new FileSystemEventHandler(ProgressFileWatcher_Created);
        
        ExiftoolBackground.DoWork += new DoWorkEventHandler(ExiftoolBackground_DoWork);
        ExiftoolBackground.ProgressChanged += new ProgressChangedEventHandler(ExiftoolBackground_ProgressChanged);
        ExiftoolBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExiftoolBackground_RunWorkerCompleted);
        ExiftoolBackground.WorkerReportsProgress = true;
        ExiftoolBackground.WorkerSupportsCancellation = true;

        RTBackground.DoWork += new DoWorkEventHandler(RTBackground_DoWork);
        RTBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RTBackground_RunWorkerCompleted);
        RTBackground.WorkerSupportsCancellation = true;

        SaveBackground.DoWork += new DoWorkEventHandler(SaveBackground_DoWork);
        SaveBackground.ProgressChanged += new ProgressChangedEventHandler(General_ProgressChanged);
        SaveBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SaveBackground_RunWorkerCompleted);
        SaveBackground.WorkerReportsProgress = true;
        SaveBackground.WorkerSupportsCancellation = true;

        OpenBackground.DoWork += new DoWorkEventHandler(OpenBackground_DoWork);
        OpenBackground.ProgressChanged += new ProgressChangedEventHandler(General_ProgressChanged);
        OpenBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OpenBackground_RunWorkerCompleted);
        OpenBackground.WorkerReportsProgress = true;
        OpenBackground.WorkerSupportsCancellation = true;        
    }

    private bool Quit(ClosingReason reason)
    {
        //the return value defines if the quiting should get cancelled or not
        if (reason != ClosingReason.Error)
        {
            ResponseType res = ResponseType.None;
            if (IsBusy())
            {
                res = UpdateInfo(InfoType.IsBusy, 0);
                if (res == ResponseType.No) { return true; }
                else if (res == ResponseType.Yes) { KillRunningProcess(); }
            }
            res = AskForSaving();
            if (res == ResponseType.Cancel) { return true; }
        }

        if (PreviewImg.Pixbuf != null) { PreviewImg.Pixbuf.Dispose(); }
        deletePPFiles(MySettings.KeepPP);

        ((IDisposable)Graph.Target).Dispose();
        ((IDisposable)Graph).Dispose();
        ((IDisposable)CurveGraph).Dispose();
        ((IDisposable)ScaleGraph).Dispose();

        System.GC.Collect();
        
        deleteThumbs();

        Application.Quit();
        return false;
    }

    private void UpdateTable()
    {
        table.Clear();

        ArrayList LScont = new ArrayList();
        int index;
        for (int i = 0; i < table.NColumns; i++) { LScont.Add("N/A"); }

        for (int i = 0; i < AllFiles.Count; i++)
        {
            //Nr
            index = (int)TableLocation.Nr;
            LScont[index] = Convert.ToString(i + 1);
            //Filenames
            index = (int)TableLocation.Filename;
            LScont[index] = AllFiles[i].Filename;
            //Brightness
            index = (int)TableLocation.Brightness;
            LScont[index] = AllFiles[i].AltBrightness.ToString("N3");
            //AV
            index = (int)TableLocation.AV;
            if (AllFiles[i].AvString != null) { LScont[index] = AllFiles[i].AvString; }
            else { LScont[index] = "N/A"; }
            //TV
            index = (int)TableLocation.TV;
            if (AllFiles[i].TvString != null) { LScont[index] = AllFiles[i].TvString; }
            else { LScont[index] = "N/A"; }
            //ISO
            index = (int)TableLocation.ISO;
            if (AllFiles[i].SvString != null) { LScont[index] = AllFiles[i].SvString; }
            else { LScont[index] = "N/A"; }
            //BV
            index = (int)TableLocation.BV;
            LScont[index] = AllFiles[i].Bv.ToString("N3");
            //Darkframes
            index = (int)TableLocation.Darkframes;
            if (String.IsNullOrEmpty(AllFiles[i].PP3.RAW.DarkFrame)) { LScont[index] = "N/A"; }
            else { LScont[index] = System.IO.Path.GetFileName(AllFiles[i].PP3.RAW.DarkFrame); }
            //Filtersets
            index = (int)TableLocation.Filtersets;
            if (AllFiles[i].Filter == null) { LScont[index] = "N/A"; }
            else { LScont[index] = AllFiles[i].Filter.Name; }
            //Keyframes
            index = (int)TableLocation.Keyframes;
            if (AllFiles[i].IsKeyframe) { LScont[index] = AllFiles[i].PP3.Name; }
            else { LScont[index] = "N/A"; }
            
            //filling the table
            table.AppendValues(LScont[0], LScont[1], LScont[2], LScont[3], LScont[4], LScont[5], LScont[6], LScont[7], LScont[8], LScont[9]);
        }
    }

    private void FillCurveSelectBox()
    {
        CellRendererText renderer = new CellRendererText();
        ListStore Curvelist = new ListStore(typeof(string));
        CurveSelectBox.PackStart(renderer, false);
        FinishedDoBox.AddAttribute(renderer, "text", 0);

        string[] names = AllCurves.GetAllCurvenames();
        int c = 0;
        if (names[names.Length - 1] == "Exposure_Compensation")
        {
            Curvelist.AppendValues(names[names.Length - 1]);
            c = 1;
        }

        for (int i = 0; i < names.Length - c; i++)
        {
            Curvelist.AppendValues(names[i]);
        }

        CurveSelectBox.Model = Curvelist;

        TreeIter iter;
        if (Curvelist.GetIterFirst(out iter)) { CurveSelectBox.SetActiveIter(iter); }
    }
    
    private void ReportError(string name, Exception exception)
    {
        Gtk.Application.Invoke(delegate
        {
            try
            {
                SaveButton.Sensitive = false;
                BackCounter.Stop();
                ProgressFileWatcher.EnableRaisingEvents = false;
                ProgressBar.Fraction = 0;
                TimeLabel.Text = "0h 0m 0s left";

                InfoLabel.Text = "Error at: " + name;
            }
            catch { /*Quit(ClosingReason.Error);*/ }

            if (!ErrorReport.ReportError(name, exception)) { Quit(ClosingReason.Error); }
        });
    }

    private ResponseType UpdateInfo(InfoType reason, int level)
    {
        ResponseType result = ResponseType.None;
        switch (reason)
        {
            case InfoType.PPFile:
                switch (level)
                {
                    case 0:
                        if (MySettings.Program == 0)
                        {
                            result = MessageBox.Show("You didn´t choose a Main Postprocessing Profile!", MessageType.Info, ButtonsType.Ok);
                            InfoLabel.Text = "Please open a Main Postprocessing Profile!";
                        }
                        else if (MySettings.Program == 1)
                        {
                            result = MessageBox.Show("Couldn´t find XMP data!", MessageType.Info, ButtonsType.Ok);
                            InfoLabel.Text = "Please add XMP data to images!";
                        }
                        else { goto default; }
                        break;
                    case 1:
                        if (MySettings.Program == 0) { InfoLabel.Text = "Opened Postprocessing Profile!"; }
                        else if (MySettings.Program == 1) { InfoLabel.Text = "Loaded XMP Data!"; }
                        else { goto default; }
                        break;
                    default:
                        goto default;
                }
                break;
            case InfoType.Imagecount:
                result = MessageBox.Show("There are not enough images opened!", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Not enough images!";
                break;
            case InfoType.BrCalc:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Calculating (1/3)";
                        break;
                    case 1:
                        InfoLabel.Text = "Calculating (2/3)";
                        break;
                    case 2:
                        InfoLabel.Text = "Calculating (3/3)";
                        break;
                    case 3:
                        InfoLabel.Text = "Error during Calculation!";
                        break;
                    case 4:
                        InfoLabel.Text = "Calculation Cancelled!";
                        break;
                    case 5:
                        InfoLabel.Text = "Brightness Calculated!";
                        break;
                    default:
                        InfoLabel.Text = "Calculating";
                        break;
                }
                break;
            case InfoType.Wait:
                InfoLabel.Text = "Wait Please!";
                break;
            case InfoType.Cancel:
                InfoLabel.Text = "Cancelled work!";
                break;
            case InfoType.CorruptPPFile:
                if (MySettings.Program == 0)
                {
                    result = MessageBox.Show("The Postprocessing Profile seems to be corrupt! (Compensation value too high or low)", MessageType.Error, ButtonsType.Ok);
                    InfoLabel.Text = "Corrupt Postprocessing Profile!";
                }
                else if (MySettings.Program == 1)
                {
                    result = MessageBox.Show("The XMP seems to be corrupt! (Compensation value too high or low)", MessageType.Error, ButtonsType.Ok);
                    InfoLabel.Text = "Corrupt XMP Data!";
                }
                else { goto default; }
                break;
            case InfoType.DeleteFolderError:
                result = MessageBox.Show("Couldn´t remove folder!", MessageType.Error, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t remove folder!";
                break;
            case InfoType.CreateFolderError:
                result = MessageBox.Show("Couldn´t create folder!", MessageType.Error, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t create folder!";
                break;
            case InfoType.HitRoof:
                if (MySettings.Program == 0) { result = MessageBox.Show("One or more Corrections were higher than the Limit of Raw Therapees compensation ability!" + Environment.NewLine + "Do you want to try it anyway? (could give bad results)", "Go on?", MessageType.Warning, ButtonsType.YesNo); }
                else if (MySettings.Program == 1) { result = MessageBox.Show("One or more Corrections were higher than the Limit of Lightrooms compensation ability!" + Environment.NewLine + "Do you want to try it anyway? (could give bad results)", "Go on?", MessageType.Warning, ButtonsType.YesNo); }
                else { goto default; }
                break;
            case InfoType.OpenImgError:
                result = MessageBox.Show("Couldn´t Open Preview Image", MessageType.Error, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t Open Preview Image";
                break;
            case InfoType.PPRemoveError:
                result = MessageBox.Show("Couldn´t remove PP3 File(s)!", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t remove PP3 File(s)!";
                break;
            case InfoType.ThumbRemoveError:
                result = MessageBox.Show("Couldn´t remove Thumb(s)!", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t remove Thumb(s)!";
                break;
            case InfoType.PPWriteError:
                result = MessageBox.Show("Couldn´t write new PP3 File(s)!");
                InfoLabel.Text = "Couldn´t write new PP3 File(s)!";
                break;
            case InfoType.InvalidOS:
                result = MessageBox.Show("Operating System is not supported!", MessageType.Error, ButtonsType.Ok);
                InfoLabel.Text = "Operating System is not supported!";
                break;
            case InfoType.ProgramNotFound:
                switch (level)
                {
                    case 0:
                        result = MessageBox.Show("Couldn´t find Rawtherapee!" + Environment.NewLine + "Please enter path to Rawtherapee in the Settings.", MessageType.Warning, ButtonsType.Ok);
                        InfoLabel.Text = "Couldn´t find Rawtherapee!";
                        break;
                    case 1:
                        result = MessageBox.Show("Couldn´t find Lightroom!" + Environment.NewLine + "Please enter path to Lightroom in the Settings.", MessageType.Warning, ButtonsType.Ok);
                        InfoLabel.Text = "Couldn´t find Lightroom!";
                        break;
                    case 2:
                        result = MessageBox.Show("Couldn´t find Exiftool!" + Environment.NewLine + "Please put Exiftool into the programs folder!", MessageType.Warning, ButtonsType.Ok);
                        InfoLabel.Text = "Couldn´t find Exiftool!";
                        break;
                    default:
                        goto default;
                }
                break;
            case InfoType.MoveFolderError:
                result = MessageBox.Show("Couldn´t move folder!", MessageType.Error, ButtonsType.Ok);
                InfoLabel.Text = "Couldn´t move folder!";
                break;
            case InfoType.RTWorking:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Raw Therapee is working!";
                        break;
                    case 1:
                        InfoLabel.Text = "Raw Therapee has finished";
                        break;
                    case 2:
                        InfoLabel.Text = "Cancelled RawTherapee editing!";
                        break;
                    case 3:
                        InfoLabel.Text = "RawTherapee closed before finishing!";
                        break;
                    case 4:
                        InfoLabel.Text = "Brightness Preview rendered!";
                        break;
                    default:
                        goto default;
                }
                break;
            case InfoType.ExiftoolWorking:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Getting Thumbs!";
                        break;
                    case 1:
                        InfoLabel.Text = "Getting Camera Data!";
                        break;
                    case 2:
                        InfoLabel.Text = "Getting Thumbs got Cancelled!";
                        break;
                    case 3:
                        InfoLabel.Text = "Couldn´t get Thumbs!";
                        CalculateButton.Sensitive = false;
                        break;
                    case 4:
                        InfoLabel.Text = "Got Data!";
                        break;
                    default:
                        goto default;
                }
                break;
            case InfoType.NewProject:
                InfoLabel.Text = "New Project created!";
                break;
            case InfoType.Saving:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Saving Project!";
                        break;
                    case 1:
                        InfoLabel.Text = "Project Saved!";
                        break;
                }
                break;
            case InfoType.Opening:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Opening Project!";
                        break;
                    case 1:
                        InfoLabel.Text = "Project successfully opened!";
                        break;
                }
                break;
            case InfoType.IsBusy:
                switch (level)
                {
                    case 0:
                        result = MessageBox.Show("DeSERt is still busy!" + Environment.NewLine + "Do you want to quit anyway?", MessageType.Warning, ButtonsType.YesNo);
                        InfoLabel.Text = "DeSERt is currently busy!";
                        break;
                    case 1:
                        InfoLabel.Text = "DeSERt is currently busy!";
                        break;
                }
                break;
            case InfoType.InvalidNumber:
                InfoLabel.Text = "Please enter a valid number!";
                break;
            case InfoType.HighLowValue:
                InfoLabel.Text = "Value too high or low!";
                break;
            case InfoType.Default:
                InfoLabel.Text = "Ready to do Work!";
                break;
            case InfoType.NotCalculated:
                result = MessageBox.Show("The brightness of the images is not calculated yet!", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Brightness not calculated yet!";
                break;
            case InfoType.TooDark:
                result = MessageBox.Show("One or more Images were too dark to calculate a brightness change!", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Brightness couldn´t get calculated!";
                break;
            case InfoType.LongCommand:
                switch (level)
                {
                    case 0:
                        result = MessageBox.Show("Command is too long, RT will abort!" + Environment.NewLine + "Do you want to keep the PP3 files and start RT manually?", "Command Problem", MessageType.Question, ButtonsType.YesNo);
                        break;
                    case 1:
                        result = MessageBox.Show("Command is too long, can´t start rendering preview!" + Environment.NewLine + "Select fewer images please.", "Command Problem", MessageType.Info, ButtonsType.Ok);
                        break;
                }
                break;
            case InfoType.FolderExists:
                switch (level)
                {
                    case 0:
                        result = MessageBox.Show("There are files in the selected Folder!" + Environment.NewLine + "Do you want to delete them?", MessageType.Question, ButtonsType.YesNo);
                        break;
                    case 1:
                        result = MessageBox.Show("Folder does not exist!", MessageType.Error, ButtonsType.Ok);
                        break;
                }
                break;
            case InfoType.SaveQuestion:
                result = MessageBox.Show("Do you want to save?" + Environment.NewLine + "This Project will be lost otherwise!", "Save?", MessageType.Question, ButtonsType.YesNo, true);
                break;
            case InfoType.Filterset:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Opened Filterset!";
                        break;
                    case 1:
                        InfoLabel.Text = "Removed Filterset!";
                        break;
                }
                break;
            case InfoType.Darkframe:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Opened Darkframe!";
                        break;
                    case 1:
                        InfoLabel.Text = "Removed Darkframe!";
                        break;
                }
                break;
            case InfoType.Keyframe:
                switch (level)
                {
                    case 0:
                        InfoLabel.Text = "Added Keyframe!";
                        break;
                    case 1:
                        InfoLabel.Text = "Removed Keyframe!";
                        break;
                }
                break;
            case InfoType.InconsistentCurve:
                result = MessageBox.Show("There were different Curvetypes in the PP3-Keyframes!" + Environment.NewLine + "Please use only one type of Curve for each Value.", MessageType.Info, ButtonsType.Ok);
                InfoLabel.Text = "Curvetypes were inconsistent!";
                break;
            case InfoType.PreviewCalc:
                InfoLabel.Text = "Rendering Brightness Preview!";
                break;
            case InfoType.FileRemoveError:
                result = MessageBox.Show("Couldn´t remove file(s)!" + Environment.NewLine + "Please remove them manually.", MessageType.Error, ButtonsType.Ok);
                break;
            default:
                result = MessageBox.Show("Something strange, yet unknown happened!", MessageType.Warning, ButtonsType.Ok);
                InfoLabel.Text = "Something strange, yet unknown happened!";
                break;
        }
        return result;
    }

    private void SetSaveStatus(bool isSaved)
    {
        ProjectSaved = isSaved;
        string title;
        if (MySettings.Program == 1) { title = "DeSELt"; }
        else { title = "DeSERt"; }
        if (isSaved && !String.IsNullOrEmpty(ProjectSavePath))
        {
            this.Title = title + " - " + System.IO.Path.GetFileNameWithoutExtension(ProjectSavePath);
        }
        else if (String.IsNullOrEmpty(ProjectSavePath) && isSaved)
        {
            this.Title = title + " - NewProject";
        }
        else if (String.IsNullOrEmpty(ProjectSavePath) && !isSaved)
        {
            this.Title = title + " - NewProject*";
        }
        else
        {
            this.Title = title + " - " + System.IO.Path.GetFileNameWithoutExtension(ProjectSavePath) + "*";
        }
    }

    private ResponseType AskForSaving()
    {
        ResponseType res = ResponseType.None;
        if (!ProjectSaved)
        {
            res = UpdateInfo(InfoType.SaveQuestion, 0);
            if (res == ResponseType.Yes)
            {
                StartSaveProjectWorker(true);
            }
        }
        return res;
    }

    private void StartSaveProjectWorker(bool askForPath)
    {
        ResponseType res = ResponseType.None;

        if (ProjectSaved) { return; }
        else if (!askForPath && !String.IsNullOrEmpty(ProjectSavePath)) { res = ResponseType.Accept; }
        else
        {
            FileChooserDialog fc = new FileChooserDialog("Save Project", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

            FileFilter filter = new FileFilter();
            filter.Name = "DeSERt Project";
            filter.AddMimeType("Project/depro");
            filter.AddPattern("*.depro");
            fc.AddFilter(filter);
            if (Directory.Exists(MySettings.LastProjDir)) { fc.SetCurrentFolder(MySettings.LastProjDir); }
            fc.DoOverwriteConfirmation = true;
            res = (ResponseType)fc.Run();
            if (!System.IO.Path.HasExtension(fc.Filename)) { ProjectSavePath = fc.Filename + ".depro"; }
            else { ProjectSavePath = fc.Filename; }
            fc.Destroy();
        }

        if (res == ResponseType.Accept)
        {
            MySettings.LastProjDir = System.IO.Path.GetDirectoryName(ProjectSavePath);
            MySettings.Save();
            UpdateInfo(InfoType.Saving, 0);
            SaveBackground.RunWorkerAsync();
        }
    }

    private void StartOpenProjectWorker()
    {
        if (AskForSaving() != ResponseType.Cancel)
        {
            FileChooserDialog fc = new FileChooserDialog("Open Project", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            FileFilter filter = new FileFilter();
            filter.Name = "DeSERt Project";
            filter.AddMimeType("Project/depro");
            filter.AddPattern("*.depro");
            fc.AddFilter(filter);
            if (Directory.Exists(MySettings.LastProjDir)) { fc.SetCurrentFolder(MySettings.LastProjDir); }

            if (fc.Run() == (int)ResponseType.Accept)
            {
                ResetProject();
                ProjectSavePath = fc.Filename;
                UpdateInfo(InfoType.Opening, 0);
                MySettings.LastProjDir = System.IO.Path.GetDirectoryName(fc.Filename);
                MySettings.Save();
                OpenBackground.RunWorkerAsync();
            }
            fc.Destroy();
        }
    }

    private void OpenFilter(TableChangeType type)
    {
        try
        {
            FileChooserDialog fc = new FileChooserDialog("Open Filterset", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            FileFilter filter = new FileFilter();
            filter.Name = "DeSERt Filterset";
            filter.AddMimeType("Filterset/fis");
            filter.AddPattern("*.fis");
            fc.AddFilter(filter);
            if (Directory.Exists(MySettings.LastFilterDir)) { fc.SetCurrentFolder(MySettings.LastFilterDir); }

            if (fc.Run() == (int)ResponseType.Accept)
            {
                TreeIter iter;
                table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                int index = table.GetPath(iter).Indices[0];
                string name = System.IO.Path.GetFileName(fc.Filename);
                MySettings.LastFilterDir = System.IO.Path.GetDirectoryName(fc.Filename);
                MySettings.Save();

                Filterset CurFilter;
                switch (type)
                {
                    case TableChangeType.Single:
                        AllFiles[index].Filter = Filterset.OpenFilterset(fc.Filename);
                        break;
                    case TableChangeType.TillEnd:
                        CurFilter = Filterset.OpenFilterset(fc.Filename);
                        for (int i = index; i < AllFiles.Count; i++) { AllFiles[index].Filter = CurFilter; }
                        break;
                    case TableChangeType.TillNext:
                        CurFilter = Filterset.OpenFilterset(fc.Filename);
                        for (int i = index; i < AllFiles.Count; i++)
                        {
                            if (AllFiles[index].Filter == null) { AllFiles[index].Filter = CurFilter; }
                            else { break; }
                        }
                        break;
                }
                UpdateTable();
                UpdateInfo(InfoType.Filterset, 0);
            }
            fc.Destroy();
        }
        catch (Exception ex) { ReportError("Open Filterset", ex); }
    }

    private void OpenDarkframe(TableChangeType type)
    {
        try
        {
            FileChooserDialog fc = new FileChooserDialog("Open Darkframe", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            #region Add FileFilter

            FileFilter filter = new FileFilter();
            filter.Name = "Images";
            filter.AddPattern("*.jpg");
            filter.AddPattern("*.jpeg");
            filter.AddPattern("*.CR2");
            filter.AddPattern("*.CRW");
            filter.AddPattern("*.png");
            filter.AddPattern("*.tif");
            filter.AddPattern("*.tiff");
            filter.AddPattern("*.DNG");
            filter.AddPattern("*.NEF");
            filter.AddPattern("*.x3f");
            filter.AddPattern("*.srw");
            filter.AddPattern("*.srf");
            filter.AddPattern("*.sr2");
            filter.AddPattern("*.arw");
            filter.AddPattern("*.erf");
            filter.AddPattern("*.pef");
            filter.AddPattern("*.raf");
            filter.AddPattern("*.3fr");
            filter.AddPattern("*.fff");
            filter.AddPattern("*.dcr");
            filter.AddPattern("*.dcs");
            filter.AddPattern("*.kdc");
            filter.AddPattern("*.rwl");
            filter.AddPattern("*.mrw");
            filter.AddPattern("*.mdc");
            filter.AddPattern("*.nrw");
            filter.AddPattern("*.orf");
            filter.AddPattern("*.rw2");
            fc.AddFilter(filter);
            filter = new FileFilter();
            filter.Name = "All Files";
            filter.AddPattern("*.*");
            fc.AddFilter(filter);

            #endregion

            if (Directory.Exists(MySettings.LastFilterDir)) { fc.SetCurrentFolder(MySettings.LastFilterDir); }

            if (fc.Run() == (int)ResponseType.Accept)
            {
                TreeIter iter;
                table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                int index = table.GetPath(iter).Indices[0];
                string name = System.IO.Path.GetFileName(fc.Filename);
                MySettings.LastFilterDir = System.IO.Path.GetDirectoryName(fc.Filename);
                MySettings.Save();

                switch (type)
                {
                    case TableChangeType.Single:
                        AllFiles[index].PP3.RAW.DarkFrame = fc.Filename;
                        break;
                    case TableChangeType.TillEnd:
                        for (int i = index; i < AllFiles.Count; i++) { AllFiles[index].PP3.RAW.DarkFrame = fc.Filename; }
                        break;
                    case TableChangeType.TillNext:
                        for (int i = index; i < AllFiles.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(AllFiles[index].PP3.RAW.DarkFrame)) { AllFiles[index].PP3.RAW.DarkFrame = fc.Filename; }
                            else { break; }
                        }
                        break;
                }
                UpdateTable();
                UpdateInfo(InfoType.Darkframe, 0);
            }
            fc.Destroy();
        }
        catch (Exception ex) { ReportError("Open Darkframe", ex); }
    }

    private void RemoveFilter(TableChangeType type)
    {
        try
        {
            TreeIter iter;
            table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
            int index = table.GetPath(iter).Indices[0];

            switch (type)
            {
                case TableChangeType.Single:
                    AllFiles[index].Filter = null;
                    break;
                case TableChangeType.TillEnd:
                    for (int i = index; i < AllFiles.Count; i++) { AllFiles[index].Filter = null; }
                    break;
                case TableChangeType.TillNext:
                    TreePath Tpath = table.GetPath(iter);
                    table.GetIter(out iter, Tpath);
                    string name = (string)table.GetValue(iter, (int)TableLocation.Filtersets);

                    for (int i = index; i < AllFiles.Count; i++)
                    {
                        if (AllFiles[index].Filter.Name != name) { AllFiles[index].Filter = null; }
                        else { break; }
                    }
                    break;
            }
            UpdateTable();
            UpdateInfo(InfoType.Filterset, 1);
        }
        catch (Exception ex) { ReportError("Remove Filterset", ex); }
    }

    private void RemoveDarkframe(TableChangeType type)
    {
        try
        {
            TreeIter iter;
            table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
            int index = table.GetPath(iter).Indices[0];

            switch (type)
            {
                case TableChangeType.Single:
                    AllFiles[index].PP3.RAW.DarkFrame = null;
                    break;
                case TableChangeType.TillEnd:
                    for (int i = index; i < AllFiles.Count; i++) { AllFiles[index].PP3.RAW.DarkFrame = null; }
                    break;
                case TableChangeType.TillNext:
                    TreePath Tpath = table.GetPath(iter);
                    table.GetIter(out iter, Tpath);
                    string name = (string)table.GetValue(iter, (int)TableLocation.Darkframes);

                    for (int i = index; i < AllFiles.Count; i++)
                    {
                        if (System.IO.Path.GetFileName(AllFiles[index].PP3.RAW.DarkFrame) != name) { AllFiles[index].PP3.RAW.DarkFrame = null; }
                        else { break; }
                    }
                    break;
            }
            UpdateTable();
            UpdateInfo(InfoType.Darkframe, 1);
        }
        catch (Exception ex) { ReportError("Remove Darkframe", ex); }
    }

    private void AddKeyframe()
    {
        try
        {
            if (PPfileOpened)
            {
                FileChooserDialog fc = new FileChooserDialog("Open PP3", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
                if (Directory.Exists(MySettings.LastPPDir)) { fc.SetCurrentFolder(MySettings.LastPPDir); }

                FileFilter filter = new FileFilter();
                filter.Name = "Postprocessing Profile";
                filter.AddPattern("*.pp3");
                fc.AddFilter(filter);

                if (fc.Run() == (int)ResponseType.Accept)
                {
                    TreeIter iter;
                    table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                    int index = table.GetPath(iter).Indices[0];
                    string name = System.IO.Path.GetFileName(fc.Filename);
                    MySettings.LastPPDir = System.IO.Path.GetDirectoryName(fc.Filename);
                    MySettings.Save();

                    AllFiles[index].PP3 = new PP3Values(fc.Filename);
                    AllFiles[index].IsKeyframe = true;
                    UpdateTable();

                    int c = 0;
                    if (BrightnessCalculated){ c = 1;}
                    GCurves.GCurve tmp;
                    CurveSelectBox.Model.GetIterFirst(out iter);
                    List<double> values = AllFiles[index].PP3.GetAllInterpolateValues();

                    for (int i = 0; i < CurveSelectBox.Model.IterNChildren() - c; i++)
                    {
                        if (c == 1 && i == 0) { CurveSelectBox.Model.IterNext(ref iter); }
                        AllCurves.Curves.TryGetValue((string)CurveSelectBox.Model.GetValue(iter, 0), out tmp);
                        CurveSelectBox.Model.IterNext(ref iter);
                        tmp.AddPoint(index, (float)values[i], true);
                    }

                    UpdateInfo(InfoType.Keyframe, 0);
                }
                fc.Destroy();
            }
            else { UpdateInfo(InfoType.PPFile, 0); }
        }
        catch (Exception ex) { ReportError("Open Keyframe", ex); }
    }

    private void RemoveKeyframe()
    {
        try
        {
            if (PPfileOpened)
            {
                TreeIter iter;
                table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                int index = table.GetPath(iter).Indices[0];

                if (AllFiles[index].IsKeyframe)
                {
                    if (index == 0)
                    {
                        PPfileOpened = false;
                        AllCurves.ResetPP3Curves();
                        FillCurveSelectBox();
                    }
                    else { AllFiles[index].PP3 = new PP3Values(MainPP3.Path); }

                    AllFiles[index].IsKeyframe = false;

                    UpdateTable();
                    UpdateInfo(InfoType.Keyframe, 1);
                }
            }
        }
        catch (Exception ex) { ReportError("Remove Filterset", ex); }
    }

    private void ResetTableBrightness(TableChangeType type)
    {
        try
        {
            switch (type)
            {
                case TableChangeType.Single:
                    TreeIter iter;
                    table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                    int index = table.GetPath(iter).Indices[0];
                    AllFiles[index].AltBrightness = AllFiles[index].Brightness;

                    double change = AllFiles[index].AltBrightness - AllFiles[index].Brightness;
                    AllFiles[index].AltBrightness = AllFiles[index].Brightness;

                    for (int i = index + 1; i < AllFiles.Count; i++)
                    {
                        AllFiles[i].AltBrightness -= change;
                    }
                    break;

                case TableChangeType.All:
                    for (int i = 0; i < AllFiles.Count; i++)
                    {
                        AllFiles[i].AltBrightness = AllFiles[i].Brightness;
                    }
                    break;
            }

            UpdateTable();
        }
        catch (Exception ex) { ReportError("Reset Table Brightness", ex); }
    }

    private void NewProject()
    {
        if (AskForSaving() != ResponseType.Cancel)
        {
            ResetProject();
            UpdateInfo(InfoType.NewProject, 0);
            SetSaveStatus(true);
        }
    }

    private void ResetProject()
    {
        table.Clear();
        AllFiles.Clear();
        AllCurves.Clear();
        MovePoint = new KeyValuePair<int, bool>(0, false);
        MainPP3 = new PP3Values();
        MainPP3.Name = "Main";
        SaveButton.Sensitive = true;
        CurveSelectBox.Model = new ListStore(typeof(string));
        BrightnessCalculated = false;
        PPfileOpened = false;
        CountdownRunning = false;
        BrScale.Value = 0;
        BrScaleEntry.Text = "0,00";

        if (PreviewImg.Pixbuf != null) { PreviewImg.Pixbuf.Dispose(); }
        PreviewImg.Pixbuf = null;

        //Clear Graph
        Graph.Color = new Cairo.Color(0.9, 0.9, 0.9);
        Graph.Rectangle(0, 0, GraphArea.Allocation.Width, GraphArea.Allocation.Height);
        Graph.Fill();

        deleteThumbs();
        deletePPFiles(MySettings.KeepPP);
        ProcessCancelled = false;
        ProjectSavePath = String.Empty;

        MainNotebook.Page = 0;
        SetSaveStatus(false);
        UpdateInfo(InfoType.Default, 0);
    }
    
    [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

    private void FinishedRoutines(ComputerState state)
    {
        switch (state)
        {
            case ComputerState.Nothing:
                //nothing to do here
                break;
            case ComputerState.Close:
                Gtk.Application.Invoke(delegate
                {
                    int lefttime = 30;
                    while (lefttime > 0)
                    {
                        if (ProcessCancelled) { UpdateInfo(InfoType.Default, 0); ProcessCancelled = false; return; }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        lefttime--;
                        InfoLabel.Text = "Will close DeSERt in "+ lefttime.ToString() +" seconds!";
                    }
                    InfoLabel.Text = "The end is near!";

                    Quit(ClosingReason.Program);
                });
                break;
            case ComputerState.Shutdown:
                Gtk.Application.Invoke(delegate
                {
                    int lefttime = 30;
                    while (lefttime > 0)
                    {
                        if (ProcessCancelled) { UpdateInfo(InfoType.Default, 0); ProcessCancelled = false; return; }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        lefttime--;
                        InfoLabel.Text = "Will shut down in " + lefttime.ToString() + " seconds!";
                    }
                    InfoLabel.Text = "The end is near!";

                    Process.Start("shutdown", "/s /t 0");
                });
                break;
            case ComputerState.Suspend:
                Gtk.Application.Invoke(delegate
                {
                    int lefttime = 30;
                    while (lefttime > 0)
                    {
                        if (ProcessCancelled) { UpdateInfo(InfoType.Default, 0); ProcessCancelled = false; return; }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        lefttime--;
                        InfoLabel.Text = "Will suspend in " + lefttime.ToString() + " seconds!";
                    }
                    InfoLabel.Text = "Set computer to suspend state!";

                    SetSuspendState(false, true, true);
                });
                break;
        }
    }

    #endregion GUI Subroutines


    #region Graph

    private void DrawGraphGrid()
    {
        using (Context GridGraphCtx = new Context(ScaleGraph))
        {
            GridGraphCtx.Antialias = Antialias.Subpixel;
            GridGraphCtx.Color = new Cairo.Color(0, 0, 0);
            GridGraphCtx.LineWidth = 0.6;
            GridGraphCtx.SelectFontFace("Purisa", FontSlant.Normal, FontWeight.Normal);
            GridGraphCtx.SetFontSize(10);

            ClearGraph(GridGraphCtx);

            GCurves.GCurve tmp;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

            if (tmp != null)
            {
                double min = tmp.min;
                double max = tmp.max;
                int width = GraphArea.Allocation.Width;
                int height = GraphArea.Allocation.Height;

                if (AllFiles.Count > 1)
                {
                    double Xp = GetGraphXY(new PointF(AllFiles.Count - 1, 0)).X;
                    double Xm = GetGraphXY(new PointF(0, 0)).X;

                    //drawing the X scale lines
                    double f = (Xp - Xm) / 10;

                    for (int i = 0; i <= 10; i++)
                    {
                        GridGraphCtx.MoveTo((int)(GrBorLeft + (f * i)) + 0.5, (int)(height - GrBor + 8) + 0.5);
                        GridGraphCtx.LineTo((int)(GrBorLeft + (f * i)) + 0.5, GrBor + 0.5);
                    }

                    //X-axis label
                    GridGraphCtx.MoveTo(GrBor + 12, height - 5);
                    GridGraphCtx.ShowText("1");
                    if (AllFiles.Count >= 10) { GridGraphCtx.MoveTo(Xp - 10, height - 5); }
                    else if (AllFiles.Count >= 100) { GridGraphCtx.MoveTo(Xp - 25, height - 5); }
                    else if (AllFiles.Count >= 1000) { GridGraphCtx.MoveTo(Xp - 40, height - 5); }
                    else if (AllFiles.Count >= 10000) { GridGraphCtx.MoveTo(Xp - 55, height - 5); }
                    else { GridGraphCtx.MoveTo(Xp - 5, height - 5); }
                    GridGraphCtx.ShowText(AllFiles.Count.ToString());

                    //drawing the Y scale lines
                    for (int i = 0; i <= 10; i++)
                    {
                        GridGraphCtx.MoveTo((int)(GrBorLeft - 8) + 0.5, (int)(height - GrBor - (((height - (2 * GrBor)) / 10) * i)) + 0.5);
                        GridGraphCtx.LineTo((int)Xp + 0.5, (int)(height - GrBor - (((height - (2 * GrBor)) / 10) * i)) + 0.5);
                    }

                    //Y-axis label
                    GridGraphCtx.MoveTo(3, height - GrBor);
                    GridGraphCtx.ShowText(min.ToString("N2"));
                    GridGraphCtx.MoveTo(3, GrBor);
                    GridGraphCtx.ShowText(max.ToString("N2"));
                    GridGraphCtx.Stroke();
                }
            }
        }
    }

    private void DrawGraphCurve()
    {
        if (MainNotebook.CurrentPage == 1 && CurveSelectBox.Active != -1)
        {
            GCurves.GCurve SelCurve;
            AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out SelCurve);

            using (Context CurveGraphCtx = new Context(CurveGraph))
            {
                CurveGraphCtx.Antialias = Antialias.Subpixel;
                CurveGraphCtx.LineWidth = 1;

                ClearGraph(CurveGraphCtx);
                List<PointF> MovePoints = new List<PointF>();

                //drawing the Brightness Input
                if (AllCurves.BrInCurve != null && CurveSelectBox.ActiveText == GCurves.CurveName.Exposure_Compensation.ToString())
                {
                    CurveGraphCtx.Color = new Cairo.Color(0, 0, 1);
                    CurveGraphCtx.LineWidth = 0.8;
                    for (int i = 0; i < AllCurves.BrInCurve.Points.Count; i++)
                    {
                        MovePoints.Add(GetGraphXY(AllCurves.BrInCurve.Points[i].Value));
                    }
                    CurveGraphCtx.MoveTo(MovePoints[0].X, MovePoints[0].Y);
                    for (int i = 1; i < MovePoints.Count; i++)
                    {
                        CurveGraphCtx.LineTo(MovePoints[i].X, MovePoints[i].Y);
                    }
                    CurveGraphCtx.Stroke();
                }

                if (SelCurve.Points.Count >= 2)
                {
                    //Drawing the output curve
                    CurveGraphCtx.Color = new Cairo.Color(0, 1, 0);
                    PointF[] DrawPoints = new PointF[200];
                    GetGraphXY(SelCurve.GetSmoothCurve(200)).CopyTo(DrawPoints);
                    List<PointF> Cpoints = new List<PointF>();
                    for (int i = 0; i < SelCurve.Points.Count; i++)
                    {
                        Cpoints.Add(GetGraphXY(SelCurve.Points[i].Value));
                    }

                    CurveGraphCtx.MoveTo(DrawPoints[0].X, DrawPoints[0].Y);

                    for (int i = 0; i < DrawPoints.Length; i++)
                    {
                        CurveGraphCtx.LineTo(DrawPoints[i].X, DrawPoints[i].Y);
                    }
                    CurveGraphCtx.Stroke();

                    
                    //Draw the activated point
                    CurveGraphCtx.Color = new Cairo.Color(0.8, 0.8, 0);
                    CurveGraphCtx.Arc(Cpoints[SelCurve.SelectedPoint].X, Cpoints[SelCurve.SelectedPoint].Y, 3, 0, 2 * Math.PI);
                    CurveGraphCtx.Fill();

                    //Draw the control points
                    CurveGraphCtx.Color = new Cairo.Color(0, 0, 0);
                    for (int i = 0; i < SelCurve.Points.Count; i++)
                    {
                        if (i != SelCurve.SelectedPoint && !SelCurve.Points[i].Key ) { CurveGraphCtx.Arc(Cpoints[i].X, Cpoints[i].Y, 3, 0, 2 * Math.PI); CurveGraphCtx.Fill(); }
                        else if (SelCurve.Points[i].Key)
                        {
                            CurveGraphCtx.Save();
                            CurveGraphCtx.Color = new Cairo.Color(0.8, 0, 0);
                            CurveGraphCtx.Arc(Cpoints[i].X, Cpoints[i].Y, 3, 0, 2 * Math.PI);
                            CurveGraphCtx.Fill();
                            CurveGraphCtx.Restore();
                        }
                    }

                    //Fill the Valueboxes with the current value
                    XValBox.Text = (SelCurve.Points[SelCurve.SelectedPoint].Value.X + 1).ToString();
                    YValBox.Text = SelCurve.Points[SelCurve.SelectedPoint].Value.Y.ToString();
                }
            }
        }
    }

    private List<PointF> GetGraphXY(List<PointF> InPoints)
    {
        List<PointF> OutList = new List<PointF>();

        for (int i = 0; i < InPoints.Count; i++)
        {
            OutList.Add(GetGraphXY(InPoints[i]));
        }

        return OutList;
    }

    private List<PointF> GetGraphXY(PointF[] InPoints)
    {
        List<PointF> pts = new List<PointF>();
        pts.AddRange(InPoints);
        return GetGraphXY(pts);
    }

    private PointF GetGraphXY(PointF InPoint)
    {
        GCurves.GCurve tmp;
        AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

        List<PointF> OutList = new List<PointF>();
        double min = tmp.min;
        double max = tmp.max;
        int Areaheight = GraphArea.Allocation.Height - (2 * GrBor);
        int Areawidth = GraphArea.Allocation.Width - GrBorLeft - GrBor;

        float x = (float)((InPoint.X / (AllFiles.Count - 1)) * Areawidth) + GrBorLeft;
        float y = (float)(((100 - (((InPoint.Y - min) * 100) / (max - min))) * Areaheight) / 100) + GrBor;
        return new PointF(x, y);
    }

    private PointF GetValueFromXY(float InX, float InY)
    {
        GCurves.GCurve tmp;
        AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

        double min = tmp.min;
        double max = tmp.max;
        int Areaheight = GraphArea.Allocation.Height - (2 * GrBor);
        int Areawidth = GraphArea.Allocation.Width - GrBorLeft - GrBor;

        float Y = (float)GraphArea.Allocation.Height - InY;

        float x = (float)(((AllFiles.Count - 1) * (InX - GrBorLeft)) / Areawidth);
        float y = (float)(((Areaheight * max) + ((GrBor - Y) * (max - min))) / Areaheight);

        return new PointF(x, y);
    }

    private void RefreshGraph(bool RefreshScale)
    {
        if (AllFiles.Count > 1 && BrightnessCalculated && Graph != null)
        {
            DrawGraphCurve();
            DrawGraphGrid();
            //if (RefreshScale) { DrawGraphGrid(); }

            //Draw all Graph-Parts together:
            using (Context GraphHelper = new Context(MainGraph))
            {
                //Clear
                ClearGraph(GraphHelper);

                //Draw a gray Background
                GraphHelper.Color = new Cairo.Color(0.9, 0.9, 0.9);
                GraphHelper.Rectangle(0, 0, GraphArea.Allocation.Width, GraphArea.Allocation.Height);
                GraphHelper.Fill();

                //Draw the Scale and Graph
                GraphHelper.SetSourceSurface(ScaleGraph, 0, 0);
                GraphHelper.Paint();
                GraphHelper.SetSourceSurface(CurveGraph, 0, 0);
                GraphHelper.Paint();
            }

            //Set the complete Graph
            Graph.SetSourceSurface(MainGraph, 0, 0);
            Graph.Paint();
        }
    }

    private void UpdateValueBox()
    {
        GCurves.GCurve tmp;
        AllCurves.Curves.TryGetValue(CurveSelectBox.ActiveText, out tmp);

        if (tmp.Points.Count > 0 && MovePoint.Value == false && !tmp.Points[tmp.SelectedPoint].Key)
        {
            double min = tmp.min;
            double max = tmp.max;
            float Xval = 0;
            float Yval = 0;

            try
            {
                Xval = (float)Convert.ToDouble(XValBox.Text.Replace('.', ','));
                Yval = (float)Convert.ToDouble(YValBox.Text.Replace('.', ','));
            }
            catch { UpdateInfo(InfoType.InvalidNumber, 0); return; }

            if (Xval < 0 || Xval > AllFiles.Count - 1 || Yval < min || Yval > max) { UpdateInfo(InfoType.HighLowValue, 0); return; }
            else { UpdateInfo(InfoType.Default, 0); }

            if (MovePoint.Key != 1 && Xval == 1) { UpdateInfo(InfoType.HighLowValue, 0); return; }
            else { UpdateInfo(InfoType.Default, 0); }

            if (MovePoint.Key != tmp.Points.Count - 1 && Xval == tmp.Points.Count - 1) { UpdateInfo(InfoType.HighLowValue, 0); return; }
            else { UpdateInfo(InfoType.Default, 0); }

            if (MovePoint.Key > 0 && MovePoint.Key < tmp.Points.Count - 1) { tmp.Points[tmp.SelectedPoint] = new KeyValuePair<bool,PointF>(tmp.Points[tmp.SelectedPoint].Key, new PointF(Xval, Yval)); }
            else { tmp.Points[tmp.SelectedPoint] = new KeyValuePair<bool,PointF>(tmp.Points[tmp.SelectedPoint].Key, new PointF(tmp.Points[tmp.SelectedPoint].Value.X, Yval)); }

            RefreshGraph(false);
        }
    }

    private void ClearGraph(Context ctx)
    {
        ctx.Save();
        ctx.Operator = Operator.Clear;
        ctx.Paint();
        ctx.Restore();
    }

    #endregion Graph


    #region Program Subroutines

    private bool IsSafe()
    {
        bool isSafe = true;

        if (AllFiles.Count < 2)
        {
            UpdateInfo(InfoType.Imagecount, 0);
            isSafe = false;
        }
        else if (!PPfileOpened)
        {
            UpdateInfo(InfoType.PPFile, 0);
            isSafe = false;
        }
        else if (!BrightnessCalculated)
        {
            UpdateInfo(InfoType.NotCalculated, 0);
            isSafe = false;
        }
        else { isSafe = checkPrograms(); }

        return isSafe;
    }

    private bool HitRoof()
    {
        bool hitroof = false;

        for (int i = 0; i < AllFiles.Count; i++)
        {
            if (MySettings.Program == 0) { if (AllFiles[i].PP3.Exposure.Compensation > 10 || AllFiles[i].PP3.Exposure.Compensation < -5) { hitroof = true; break; } }
            else if (MySettings.Program == 1) { }
        }

        return hitroof;
    }

    private void KillRunningProcess()
    {
        //Killing Rawtherapee if running
        try
        {
            if (RTBackground.IsBusy == true && RT.HasExited == false)
            {
                RT.Kill();
                RT.WaitForExit();
            }
        }
        catch (InvalidOperationException) { }

        //Killing Exiftool if running
        try
        {
            if (ExiftoolBackground.IsBusy == true && exiftool.HasExited == false)
            {
                exiftool.Kill();
                exiftool.WaitForExit();
            }
        }
        catch (InvalidOperationException) { }

        //Killing all Calculation Threads if running
        try
        {
            for (int i = 0; i < MySettings.Threads; i++)
            {
                if (CalcWorker.Count > 0)
                {
                    if (CalcWorker[i].IsBusy == true && CalcWorker[i].CancellationPending == false)
                    {
                        CalcWorker[i].CancelAsync();
                    }
                }
            }
        }
        catch { }

        if (ExiftoolBackground.IsBusy || RTBackground.IsBusy || CountdownRunning) { ProcessCancelled = true; }
    }

    private bool IsBusy()
    {
        if (RTBackground.IsBusy || ExiftoolBackground.IsBusy || SaveBackground.IsBusy || OpenBackground.IsBusy) 
        { UpdateInfo(InfoType.IsBusy, 1); return true; }

        for (int i = 0; i < CalcWorker.Count; i++)
        {
            if (CalcWorker[i].IsBusy) { UpdateInfo(InfoType.IsBusy, 1); return true; }
        }

        return false;
    }
    
    private void deletePPFiles(bool keep)
    {
        if (keep == false && AllFiles.Count > 0)
        {
            for (int i = 0; i < AllFiles.Count; i++)
            {
                string pp3file = AllFiles[i].FilePath + ".pp3";

                if (File.Exists(pp3file))
                {
                    try { File.Delete(pp3file); }
                    catch { UpdateInfo(InfoType.PPRemoveError, 0); return; }
                }
            }
        }
    }

    private void deleteThumbs()
    {
        if (Directory.Exists(Thumbpath))
        {
            string[] files = Directory.GetFiles(Thumbpath);
            bool error = false;
            foreach (string path in files)
            {
                int count = 0;
                while (count < 5)
                {
                    try { File.Delete(path); break; }
                    catch { count++; Thread.Sleep(20); }
                }
                if (count == 5) { error = true; }
            }
            if (error) 
            {
                error = false;
                int count = 0;
                while (count < 5)
                {
                    try { Directory.Delete(Thumbpath); break; }
                    catch { count++; Thread.Sleep(20); }
                }
                if (count == 5) { error = true; }

                if (error) { UpdateInfo(InfoType.ThumbRemoveError, 0); }
                else { Directory.CreateDirectory(Thumbpath); }
            }
        }
    }

    private bool InterpolatePP3()
    {
        CalculateNewCompensationValue();

        List<int> keyframes = new List<int>();
        List<KeyValuePair<int, double[]>> PP3Curves = new List<KeyValuePair<int, double[]>>();
        List<List<KeyValuePair<int, double[]>>> KeyCurves = new List<List<KeyValuePair<int, double[]>>>();
        List<int> Dimension = new List<int>();
        List<double> expcomp = new List<double>();
        for (int i = 0; i < AllFiles.Count; i++)
        {
            if (AllFiles[i].IsKeyframe || i == AllFiles.Count)
            {
                PP3Curves = AllFiles[i].PP3.GetAllCurves();
                for (int j = 0; j < PP3Curves.Count; j++)
                {
                    if (KeyCurves.Count != PP3Curves.Count) { KeyCurves.Add(new List<KeyValuePair<int, double[]>>()); }
                    KeyCurves[j].Add(PP3Curves[j]);

                    expcomp.Add(AllFiles[i].PP3.Exposure.Compensation);

                    //if curvetype is inconsistent return and report error:
                    if (keyframes.Count > 0) { if (KeyCurves[j][keyframes.Count - 1].Key != KeyCurves[j][keyframes.Count].Key) { UpdateInfo(InfoType.InconsistentCurve, 0); return false; } }

                    //curvetypes -1 and 2 are ondimensional, 1, 3 and 4 are twodimensional
                    if (Dimension.Count != PP3Curves.Count)
                    {
                        if (PP3Curves[j].Key == 0) { Dimension.Add(0); }
                        else if (PP3Curves[j].Key == -1 || PP3Curves[j].Key == 2) { Dimension.Add(1); }
                        else if (PP3Curves[j].Key == 1 || PP3Curves[j].Key == 3 || PP3Curves[j].Key == 4) { Dimension.Add(2); }
                    }
                }
                keyframes.Add(i);
            }
        }

        if (keyframes.Count > 1)
        {
            string[] names = AllCurves.GetAllCurvenames();

            //Normal PP3 values
            List<List<double>> PP3vals = new List<List<double>>();
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] != "Exposure_Compensation")
                {
                    var tmp = AllCurves.Curves[names[i]].GetSmoothCurve(AllFiles.Count);
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        if (PP3vals.Count != AllFiles.Count) { PP3vals.Add(new List<double>()); }
                        PP3vals[j].Add(tmp[j].Y);
                    }
                }
            }

            //PP3 Curves
            List<List<double[]>> PP3CurvesInterpolated = new List<List<double[]>>();
            for (int i = 0; i < KeyCurves.Count; i++)
            {
                if (Dimension[i] == 0)
                {
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        if (PP3CurvesInterpolated.Count != AllFiles.Count) { PP3CurvesInterpolated.Add(new List<double[]>()); }
                        PP3CurvesInterpolated[j].Add(new double[0]);
                    }
                }
                else if (Dimension[i] == 1)
                {
                    //reorder list so the to interpolate values are in one row
                    List<List<double>> ValList = new List<List<double>>();
                    int length = KeyCurves[i][0].Value.Length;
                    for (int j = 0; j < length; j++)
                    {
                        ValList.Add(new List<double>());
                        for (int k = 0; k < keyframes.Count; k++)
                        {
                            ValList[j].Add(KeyCurves[i][k].Value[j]);
                        }
                    }

                    //interpolate keyframevalues for all images
                    List<List<double>> Interpol = new List<List<double>>();
                    for (int j = 0; j < ValList.Count; j++)
                    {
                        Interpol.Add(GetSmoothCurve(ValList[j], AllFiles.Count));
                    }

                    //reorder list back to normal
                    List<double[]> InterpolRe = new List<double[]>();
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        InterpolRe.Add(new double[length]);

                        for (int l = 0; l < length; l++)
                        {
                            InterpolRe[j][l] = Interpol[l][j];
                        }
                    }

                    //store to final list
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        if (PP3CurvesInterpolated.Count != AllFiles.Count) { PP3CurvesInterpolated.Add(new List<double[]>()); }
                        if (PP3CurvesInterpolated[j].Count != KeyCurves.Count) { PP3CurvesInterpolated[j].Add(new double[length]); }
                        PP3CurvesInterpolated[j][i] = InterpolRe[j];
                    }
                }
                else
                {
                    //make a list that has all points for every keyframe
                    List<List<double>> XList = new List<List<double>>();
                    List<List<double>> YList = new List<List<double>>();
                    for (int j = 0; j < keyframes.Count; j++)
                    {
                        XList.Add(new List<double>());
                        YList.Add(new List<double>());
                        for (int k = 0; k < KeyCurves[i][j].Value.Length; k++)
                        {
                            XList[j].Add(KeyCurves[i][j].Value[k]);
                            k++;
                            YList[j].Add(KeyCurves[i][j].Value[k]);
                        }
                    }

                    //make a consistent number of points for all keyframes
                    int max = XList.Max(p => p.Count);
                    for (int j = 0; j < keyframes.Count; j++)
                    {
                        XList[j] = GetSmoothCurve(XList[j], max);
                        YList[j] = GetSmoothCurve(YList[j], max);
                    }

                    //reorder list so the to interpolate values are in one row
                    List<List<PointF>> XValList = new List<List<PointF>>();
                    List<List<PointF>> YValList = new List<List<PointF>>();
                    for (int j = 0; j < max; j++)
                    {
                        XValList.Add(new List<PointF>());
                        YValList.Add(new List<PointF>());
                        for (int k = 0; k < keyframes.Count; k++)
                        {
                            XValList[j].Add(new PointF(keyframes[k], (float)XList[k][j]));
                            YValList[j].Add(new PointF(keyframes[k], (float)YList[k][j]));
                        }
                    }

                    //interpolate keyframevalues for all images
                    List<List<PointF>> InterpolX = new List<List<PointF>>();
                    List<List<PointF>> InterpolY = new List<List<PointF>>();
                    for (int j = 0; j < max; j++)
                    {
                        InterpolX.Add(GetSmoothCurve(XValList[j], AllFiles.Count));
                        InterpolY.Add(GetSmoothCurve(YValList[j], AllFiles.Count));
                    }


                    //reorder list back to normal
                    List<double[]> InterpolRe = new List<double[]>();
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        InterpolRe.Add(new double[max * 2]);

                        int ic = 0;
                        for (int l = 0; l < max; l++)
                        {
                            InterpolRe[j][ic] = InterpolX[l][j].Y;
                            ic++;
                            InterpolRe[j][ic] = InterpolY[l][j].Y;
                            ic++;
                        }
                    }

                    //store to final list
                    for (int j = 0; j < AllFiles.Count; j++)
                    {
                        if (PP3CurvesInterpolated.Count != AllFiles.Count) { PP3CurvesInterpolated.Add(new List<double[]>()); }
                        if (PP3CurvesInterpolated[j].Count != KeyCurves.Count) { PP3CurvesInterpolated[j].Add(new double[max]); }
                        PP3CurvesInterpolated[j][i] = InterpolRe[j];
                    }
                }
            }

            List<double> expcompInterpolated = GetSmoothCurve(expcomp, AllFiles.Count);

            for (int i = 0; i < AllFiles.Count; i++)
            {
                AllFiles[i].PP3.SetAllInterpolateValues(PP3vals[i]);
                AllFiles[i].PP3.SetAllCurves(PP3CurvesInterpolated[i]);
                AllFiles[i].PP3.Exposure.Compensation = expcompInterpolated[i];
            }
        }

        return true;
    }

    public List<double> GetSmoothCurve(List<double> InValues, int outputCount)
    {
        double[] tmp = new double[InValues.Count];
        InValues.CopyTo(tmp);
        return GetSmoothCurve(tmp, outputCount);
    }

    public List<double> GetSmoothCurve(double[] InValues, int outputCount)
    {
        List<PointF> Points = new List<PointF>();
        List<double> OutPoints = new List<double>();

        for (int i = 0; i < InValues.Length; i++) { Points.Add(new PointF(i, (float)InValues[i])); }

        Points = GetSmoothCurve(Points, outputCount);

        for (int i = 0; i < outputCount; i++) { OutPoints.Add(Points[i].Y); }

        return OutPoints;
    }

    public List<PointF> GetSmoothCurve(List<PointF> InPoints, int outputCount)
    {
        PointF[] TmpCalcPoints;
        List<PointF> OutPoints = new List<PointF>();

        if (InPoints.Min(p => p.X) == InPoints.Max(p => p.X) && InPoints.Min(p => p.Y) == InPoints.Max(p => p.Y))
        {
            for (int i = 0; i < outputCount; i++) { OutPoints.Add(InPoints[0]); }
            return OutPoints;
        }
        else if (InPoints.Count != outputCount)
        {
            int factor;
            if (outputCount > 60 || InPoints.Count == 2) { factor = 1; }
            else if (outputCount > 20) { factor = 5; }
            else if (outputCount > 10) { factor = 7; }
            else { factor = 15; }

            MySpline.CalcSpline(InPoints, outputCount * factor, out TmpCalcPoints);

            factor = Convert.ToInt32(Math.Floor((double)TmpCalcPoints.Length / ((double)outputCount - 1)));

            for (int i = 0; i < outputCount; i++) { OutPoints.Add(TmpCalcPoints[i * factor]); }

            OutPoints[outputCount - 1] = InPoints[InPoints.Count - 1];

            return OutPoints;
        }
        else { return InPoints; }
    }

    private void CalculateNewCompensationValue()
    {
        PointF[] SmoothBrOut = AllCurves.Curves[GCurves.CurveName.Exposure_Compensation.ToString()].GetSmoothCurve(AllFiles.Count);
        
        for (int i = 0; i < AllFiles.Count; i++)
        {
            //Calculate the new Compensation Value:
            AllFiles[i].PP3.NewCompensation = Math.Log(SmoothBrOut[i].Y / AllFiles[i].AltBrightness, 2) + AllFiles[i].PP3.Exposure.Compensation;
            if (double.IsNaN(AllFiles[i].PP3.NewCompensation)) { throw new NotFiniteNumberException(); }
        }
    }

    private void writePPFiles()
    {
       for (int i = 0; i < AllFiles.Count; i++)
       {
           string path = AllFiles[i].FilePath + ".pp3";
           AllFiles[i].PP3.Exposure.Auto = false;
           AllFiles[i].PP3.WriteFile(path);
       }
    }

    //not written yet
    private void writeXMPFiles()
    {
    }

    //incomplete (check for Unix and Mac)
    private bool checkPrograms()
    {
        bool issafe = true;
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (File.Exists(MySettings.ProgramPath) == false) { UpdateInfo(InfoType.ProgramNotFound, MySettings.Program); issafe = false; }
            if (File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe")) == false) { UpdateInfo(InfoType.ProgramNotFound, 2); issafe = false; }
        }
        else if (Environment.OSVersion.Platform == PlatformID.Unix) { issafe = true; }
        else if (Environment.OSVersion.Platform == PlatformID.MacOSX) { issafe = true; }
        else { UpdateInfo(InfoType.InvalidOS, 0); issafe = false; }
        return issafe;
    }

    private void StartCalcThreads()
    {
        FinishedThreads = new int[] { 0, 0, 0, 0, 0 };
        CalcWorker.Clear();
        WorkerHasError = false;

        for (int f = 0; f < AllFiles.Count; f++) { AllFiles[f].Brightness = double.NaN; }

        for (int i = 0; i < MySettings.Threads; i++)
        {
            CalcWorker.Add(new BackgroundWorker());
            CalcWorker[i].WorkerSupportsCancellation = true;
            CalcWorker[i].WorkerReportsProgress = true;
            CalcWorker[i].DoWork += new DoWorkEventHandler(StaticMaskBackgroundV21_DoWork);
            
            CalcWorker[i].ProgressChanged += new ProgressChangedEventHandler(CalculationsBackground_ProgressChanged);
            CalcWorker[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(CalculationsBackground_RunWorkerCompleted);
            CalcWorker[i].RunWorkerAsync(i);
        }

        ProgressPulse.Start();
        BackCounter.Start();
    }

    private void StartRT(string OutputPath)
    {
        //Put the correct command together to start RawTherapee
        string saveformat = "-j[" + MySettings.JPGQuality + "]";
        string SaveFileType = string.Empty;
        switch (MySettings.SavingFormat)
        {
            case 0:
                saveformat = "-j[" + MySettings.JPGQuality + "]";
                SaveFileType = "jpg";
                break;
            case 1:
                saveformat = "-n";
                SaveFileType = "png";
                break;
            case 2:
                if (MySettings.TiffCompress == false) { saveformat = "-t"; }
                else { saveformat = "-t1"; }
                SaveFileType = "tiff";
                break;
            default:
                saveformat = "-j[" + MySettings.JPGQuality + "]";
                SaveFileType = "jpg";
                break;
        }

        string command = " -o " + "\"" + OutputPath + "\"" + " -S " + saveformat + " -c " + "\"" + Workpath + "\"";

        //the windows command line can´t take longer commands than 8191 characters
        if (command.Length > 8191 && Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            ResponseType res = UpdateInfo(InfoType.LongCommand, 0);
            if (res == ResponseType.Yes) { MySettings.KeepPP = true; }
            return;
        }

        UpdateInfo(InfoType.RTWorking, 0);

        BackCounter.Start();
        ProgressPulse.Start();

        ProgressFileWatcher.Filter = "*." + SaveFileType;
        ProgressFileWatcher.Path = OutputPath;
        ProgressFileWatcher.EnableRaisingEvents = true;

        RTBackground.RunWorkerAsync(command);
    }

    //not written yet
    private void StartLR()
    {
        InfoLabel.Text = "Lightroom is not supportet yet!";
    }

    private void GetExifData()
    {
        UpdateInfo(InfoType.ExiftoolWorking, 0);

        //Put the correct command together to start exiftool
        string command = "-thumbnailimage -b -w " + "\"" + Thumbpath + "%f_Thumb.jpg" + "\" " + "\"" + Workpath + "\"";

        //check if exif data (from normal image, eg jpg) is available, if it´s a raw, an exception will happen
        try
        {
            System.Drawing.Image img = new Bitmap(AllFiles[0].FilePath);
            System.Drawing.Imaging.PropertyItem[] items = img.PropertyItems;
            if (items.Length < 15) { command = "NoExif"; }
        }
        catch { }

        if (!Directory.Exists(Thumbpath)) { Directory.CreateDirectory(Thumbpath); }

        if (Directory.GetFiles(Thumbpath).Length > 0) { return; }

        ProgressFileWatcher.Filter = "*.jpg";
        ProgressFileWatcher.Path = Thumbpath;
        ProgressFileWatcher.EnableRaisingEvents = true;

        ExiftoolBackground.RunWorkerAsync(command);
    }

    private void RenderPreview()
    {
        int PrevIndex = (int)PrevIndexSpin.Value - 1;

        if (AllFiles.Count > 1 && BrightnessCalculated && AllFiles[PrevIndex].Height > 0)
        {
            ProgState = ProgramState.PreviewRender;
            int width, height;
            double fac = AllFiles[PrevIndex].Width / AllFiles[PrevIndex].Height;
            if (AllFiles[PrevIndex].Width > AllFiles[PrevIndex].Height)
            {
                width = 600;
                height = (int)(width * fac);
            }
            else
            {
                height = 400;
                width = (int)(height * fac);
            }

            string prevPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "preview");
            string command = " -o " + "\"" + prevPath + "\"" + " -S " + "-j[95]" + " -c";

            if (InterpolatePP3())
            {
                for (int i = 0; i < PrevCountSpin.Value; i++)
                {
                    PP3Values neutral = new PP3Values();
                    neutral.Resize.Width = width;
                    neutral.Resize.Height = height;
                    neutral.Resize.Enabled = true;
                    neutral.VignettingCorrection = AllFiles[PrevIndex + i].PP3.VignettingCorrection;
                    neutral.WhiteBalance = AllFiles[PrevIndex + i].PP3.WhiteBalance;
                    neutral.ColorManagement = AllFiles[PrevIndex + i].PP3.ColorManagement;
                    neutral.RAW = AllFiles[PrevIndex + i].PP3.RAW;
                    neutral.Exposure = AllFiles[PrevIndex + i].PP3.Exposure;
                    neutral.Exposure.Auto = false;
                    neutral.NewCompensation = AllFiles[PrevIndex + i].PP3.Exposure.Compensation + AllFiles[PrevIndex].PP3.NewCompensation;
                    neutral.WriteFile(AllFiles[PrevIndex + i].FilePath + ".pp3");
                    command += " \"" + AllFiles[PrevIndex + i].FilePath + "\"";
                }
            }
            else { return; }
            
            //the windows command line can´t take longer commands than 8191 characters
            if (command.Length > 8191 && Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                UpdateInfo(InfoType.LongCommand, 1);
            }

            if (Directory.Exists(prevPath))
            {
                string[] files = Directory.GetFiles(prevPath);
                if (files.Length > 0)
                {
                    foreach (string path in files) { File.Delete(path); }
                }
            }
            else { Directory.CreateDirectory(prevPath); }
            
            UpdateInfo(InfoType.PreviewCalc, 0);
            
            BackCounter.Start();

            ProgressFileWatcher.Filter = "*.jpg";
            ProgressFileWatcher.Path = prevPath;
            ProgressFileWatcher.EnableRaisingEvents = true;

            ProgressPulse.Start();

            RTBackground.RunWorkerAsync(command);
        }
    }

    private bool ThumbnailCallback()
    {
        return false;
    }

    private void ReadMainProject(MemoryStream MemStream)
    {
        BinaryFormatter bin = new BinaryFormatter();
        SaverClass sv = new SaverClass();
        sv = (SaverClass)bin.Deserialize(MemStream);
        TransferSaveVals(sv);
    }

    private void OpenProjectV2(ZipFile file)
    {
        string tmpdirectorypath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "tmp") + System.IO.Path.DirectorySeparatorChar;
        if (!Directory.Exists(tmpdirectorypath)) { Directory.CreateDirectory(tmpdirectorypath); }

        deleteThumbs();

        foreach (ZipEntry entry in file)
        {
            if (!entry.IsFile) { continue; }

            Stream zipStream = file.GetInputStream(entry);
            byte[] buffer = new byte[4096];

            if (entry.Comment == "Main")
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    StreamUtils.Copy(zipStream, stream, buffer);
                    stream.Position = 0;
                    ReadMainProject(stream);
                }
            }
            else if (entry.Comment == "Thumb")
            {
                string filesavepath = System.IO.Path.Combine(Thumbpath, entry.Name);
                using (FileStream streamWriter = File.Create(filesavepath)) { StreamUtils.Copy(zipStream, streamWriter, buffer); }
            }
            else { throw new FileLoadException("Wrong Format"); }
        }

        Directory.Delete(tmpdirectorypath);
    }

    private string SaveProject(string tmpdirectorypath)
    {
        string tmpsavepath = System.IO.Path.Combine(tmpdirectorypath, "main");
        BinaryFormatter bin = new BinaryFormatter();

        if (!Directory.Exists(tmpdirectorypath)) { Directory.CreateDirectory(tmpdirectorypath); }

        using (FileStream stream = new FileStream(tmpsavepath, FileMode.Create))
        {
            SaverClass sv = new SaverClass();
            SetSaveVals(sv);
            bin.Serialize(stream, sv);
        }

        return tmpsavepath;
    }

    private void SetSaveVals(SaverClass sv)
    {
        sv.AllCurveData = AllCurves;
        sv.AllFileData = AllFiles;
        sv.BrScaleVal = BrScale.Value;
        sv.PrevCountSpintVal = PrevCountSpin.Value;
        sv.PrevIndexSpinVal = PrevIndexSpin.Value;
        sv.Program = MySettings.Program;
        sv.Version = ProjectInfo.FullVersion;
        sv.BrightnessCalculated = BrightnessCalculated;
        sv.MainPP3 = MainPP3;
        sv.MovePoint = MovePoint;
        sv.PPfileOpened = PPfileOpened;
        sv.Workpath = Workpath;
    }

    private void TransferSaveVals(SaverClass sv)
    {
        AllCurves = sv.AllCurveData;
        AllFiles = sv.AllFileData;
        BrScale.Value = sv.BrScaleVal;
        PrevCountSpin.Value = sv.PrevCountSpintVal;
        PrevIndexSpin.Value = sv.PrevIndexSpinVal;
        MySettings.Program = sv.Program;
        MySettings.Save();
        BrightnessCalculated = sv.BrightnessCalculated;
        MainPP3 = sv.MainPP3;
        MovePoint = sv.MovePoint;
        PPfileOpened = sv.PPfileOpened;
        Workpath = sv.Workpath;

        //maybe use as a check
        //ProjectInfo.FullVersion = sv.Version;
    }

    #endregion Program Subroutines

}