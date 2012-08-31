using System;

namespace DeSERt
{
    public partial class Settings : Gtk.Dialog
    {
        private string RTpath;
        private string LRpath;

        public Settings()
        {
            try
            {
                this.Build();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    Gtk.FileFilter filter = new Gtk.FileFilter();
                    filter.Name = "Executable";
                    filter.AddMimeType("Executable/exe");
                    filter.AddPattern("*.exe");
                    ProgramPathChoose.AddFilter(filter);
                }

                KeepPPChkBox.Active = MySettings.KeepPP;
                AutothreadsChkBox.Active = MySettings.AutoThreads;
                if (MySettings.AutoThreads == true) { ThreadcountSpin.Sensitive = false; }
                else { ThreadcountSpin.Sensitive = true; }
                ThreadcountSpin.Value = MySettings.Threads;
                ProgramPathChoose.SetFilename(MySettings.ProgramPath);
                QualitySpin.Value = MySettings.JPGQuality;
                CompressChkBox.Active = MySettings.TiffCompress;
                ProgramSelBox.Active = MySettings.Program;
                RTpath = MySettings.RT;
                LRpath = MySettings.LR;
                switch (MySettings.Program)
                {
                    case 0:
                        ProgramPathLabel.LabelProp = "Path to Rawtherapee:";
                        ProgramPathChoose.SetFilename(RTpath);
                        break;

                    case 1:
                        ProgramPathLabel.LabelProp = "Path to Lightroom:";
                        ProgramPathChoose.SetFilename(LRpath);
                        break;
                }
                switch (MySettings.SavingFormat)
                {
                    case (0):
                        SavingFormatBox.Active = 0;
                        CompressChkBox.Sensitive = false;
                        return;

                    case (1):
                        SavingFormatBox.Active = 1;
                        CompressChkBox.Sensitive = false;
                        BitDepthBox.Active = MySettings.BitDepthPNG;
                        return;

                    case (2):
                        SavingFormatBox.Active = 2;
                        CompressChkBox.Sensitive = true;
                        BitDepthBox.Active = MySettings.BitDepthTiff;
                        return;
                }
            } 
            catch (Exception ex) { ErrorReport.ReportError("Init (Settings)", ex); }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            try
            {
                MySettings.ProgramPath = ProgramPathChoose.Filename;
                MySettings.KeepPP = KeepPPChkBox.Active;
                MySettings.SavingFormat = SavingFormatBox.Active;
                MySettings.JPGQuality = (int)QualitySpin.Value;
                MySettings.Threads = (int)ThreadcountSpin.Value;
                MySettings.AutoThreads = AutothreadsChkBox.Active;
                MySettings.TiffCompress = CompressChkBox.Active;
                MySettings.LR = LRpath;
                MySettings.RT = RTpath;
                MySettings.Program = ProgramSelBox.Active;
                switch (SavingFormatBox.Active)
                {
                    case 1:
                        MySettings.BitDepthPNG = BitDepthBox.Active;
                        break;
                    case 2:
                        MySettings.BitDepthTiff = BitDepthBox.Active;
                        break;
                }
                switch (MySettings.Program)
                {
                    case 0:
                        MySettings.ProgramPath = RTpath;
                        break;
                    case 1:
                        MySettings.ProgramPath = LRpath;
                        break;
                }

                MySettings.Save();

                this.Hide();
            }
            catch (Exception ex) { ErrorReport.ReportError("OK Button (Settings)", ex); }
        }

        protected void OnButtonCancelClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected void OnProgramSelBoxChanged(object sender, EventArgs e)
        {
            switch (ProgramSelBox.Active)
            {
                case 0:
                    ProgramPathLabel.LabelProp = "Path to RawTherapee:";
                    ProgramPathChoose.SetFilename(RTpath);
                    break;

                case 1:
                    MessageBox.Show("Lightroom is not supported yet, please use RawTherapee instead", Gtk.MessageType.Warning, Gtk.ButtonsType.Ok);     //temporary
                    ProgramPathLabel.LabelProp = "Path to Lightroom:";
                    ProgramPathChoose.SetFilename(LRpath);
                    break;
                default:
                    ProgramUseLabel.LabelProp = "Path to Program";
                    break;
            }
        }

        protected void OnProgramPathChooseSelectionChanged(object sender, EventArgs e)
        {
            if (ProgramSelBox.Active == 0) { RTpath = ProgramPathChoose.Filename; }
            else if (ProgramSelBox.Active == 1) { LRpath = ProgramPathChoose.Filename; }
        }

        protected void OnAutothreadsChkBoxToggled(object sender, EventArgs e)
        {
            if (AutothreadsChkBox.Active)
            {
                ThreadcountSpin.Value = Environment.ProcessorCount;
                ThreadcountSpin.Sensitive = false;
            }
            else { ThreadcountSpin.Sensitive = true; }
        }

        protected void OnSavingFormatBoxChanged(object sender, EventArgs e)
        {
            switch (SavingFormatBox.Active)
            {
                case 0:
                    QualitySpin.Sensitive = true;
                    BitDepthBox.Sensitive = false;
                    CompressChkBox.Sensitive = false;
                    break;

                case 1:
                    QualitySpin.Sensitive = false;
                    BitDepthBox.Sensitive = true;
                    CompressChkBox.Sensitive = false;
                    break;

                case 2:
                    QualitySpin.Sensitive = false;
                    BitDepthBox.Sensitive = true;
                    CompressChkBox.Sensitive = true;
                    break;
            }
        }



    }
}