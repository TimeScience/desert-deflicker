using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using Cairo;
using DeSERt;
using Gtk;

public partial class DeSERtMain
{

    #region Variables
    
    private ImageSurface MainGraph;
    private ImageSurface CurveGraph;
    private ImageSurface ScaleGraph;

    private BackgroundWorker ExiftoolBackground;
    private BackgroundWorker RTBackground;
    private BackgroundWorker SaveBackground;
    private BackgroundWorker OpenBackground;
    private BackgroundWorker ExifWorker;
    private List<BackgroundWorker> CalcWorker = new List<BackgroundWorker>();

    private Process RT = new Process();
    private Process exiftool = new Process();

    private List<FileData> AllFiles = new List<FileData>();
    private PP3Values MainPP3;
    private FileSystemWatcher ProgressFileWatcher;
    private Context Graph;
    private Exception ThreadException;
    private TimeSpan Elapsedtime = TimeSpan.Zero;
    private GCurves AllCurves;
    private MyTimer BackCounter;
    private PulseBar ProgressPulse;
    private ProgramState ProgState;
    private KeyValuePair<int, bool> MovePoint = new KeyValuePair<int, bool>(0, false);
    private ListStore table = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

    private int GrBor = 30;
    private int GrBorLeft = 45;
    private int[] FinishedThreads = new int[] { 0, 0, 0, 0, 0 };

    private long lastTime = 0;

    private string ProjectSavePath = String.Empty;
    private string Thumbpath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Thumbs") + System.IO.Path.DirectorySeparatorChar;
    private string Workpath;

    private bool ProcessCancelled = false;
    private bool ProjectSaved = true;
    private bool WorkerHasError = false;
    private bool BrightnessCalculated = false;
    private bool PPfileOpened = false;
    private bool CountdownRunning = false;
    
    #endregion Variables


    #region Enumerators

    private enum InfoType
    {
        PPFile,
        Imagecount,
        BrCalc,
        Wait,
        Cancel,
        CorruptPPFile,
        DeleteFolderError,
        CreateFolderError,
        HitRoof,
        OpenImgError,
        Saving,
        Opening,
        NewProject,
        PPRemoveError,
        ThumbRemoveError,
        PPWriteError,
        InvalidOS,
        ProgramNotFound,
        MoveFolderError,
        RTWorking,
        ExiftoolWorking,
        ExiftoolThumbError,
        OK,
        ProcessCancelled,
        Exception,
        IsBusy,
        InvalidNumber,
        HighLowValue,
        Default,
        NotCalculated,
        Error,
        TooDark,
        FolderExists,
        LongCommand,
        SaveQuestion,
        Filterset,
        Darkframe,
        Keyframe,
        InconsistentCurve,
        PreviewCalc,
        FileRemoveError,

        None = 99,
    }

    private enum ClosingReason
    {
        Error,
        User,
        Program,
    }

    private enum CalcState
    {
        Mask,
        Brightness,
        Statistics,
        CompareBrwithBv,
    }

    private enum TableChangeType
    {
        Single,
        TillEnd,
        TillNext,
        All,
    }

    private enum TableLocation
    {
        Nr,
        Filename,
        Brightness,
        AV,
        TV,
        ISO,
        BV,
        Darkframes,
        Filtersets,
        Keyframes,
    }

    private enum ComputerState
    {
        Nothing,
        Close,
        Suspend,
        Shutdown,
    }

    private enum ProgramState
    {
        Idle,
        PreviewRender,
        RTRender,
        GetExif,
        BrightnessCalc,
    }

    #endregion

}