using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DeSERt
{
    public static class MySettings
    {
        public static string ProgramPath { get; set; }
        public static string RT { get; set; }
        public static string LR { get; set; }
        public static string LastProjDir { get; set; }
        public static string LastPPDir { get; set; }
        public static string LastPicDir { get; set; }
        public static string LastFilterDir { get; set; }
        public static string LastSaveDir { get; set; }
        public static int Threads { get; set; }
        public static int Program { get; set; }
        public static int JPGQuality { get; set; }
        public static int SavingFormat { get; set; }
        public static int BitDepthPNG { get; set; }
        public static int BitDepthTiff { get; set; }
        public static bool KeepPP { get; set; }
        public static bool TiffCompress { get; set; }
        public static bool AutoThreads { get; set; }

        private static string SettingsPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User.stgs");


        public static void Load()
        {
            try
            {
                SetDefaultValues();

                using (FileStream stream = new FileStream(SettingsPath, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    HelperClass hc = (HelperClass)bin.Deserialize(stream);
                    hc.PushValues();
                }
            }
            catch (Exception ex)
            {
                Gtk.MessageDialog md = new Gtk.MessageDialog(null, Gtk.DialogFlags.DestroyWithParent, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, "Couldn´t read Settings!" + Environment.NewLine + ex.Message);
                md.Title = "Error";
                md.AddButton("Restore Settings?", Gtk.ResponseType.Yes);
                Gtk.ResponseType result = (Gtk.ResponseType)md.Run();
                md.Destroy();

                if (result == Gtk.ResponseType.Yes) { Recreate(); }
            }
        }

        private static void Recreate()
        {
            try
            {
                SetDefaultValues();

                using (FileStream stream = new FileStream(SettingsPath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    HelperClass hc = new HelperClass();
                    bin.Serialize(stream, hc);
                }
            }
            catch (Exception ex) { MessageBox.Show("Couldn´t restore Settings!" + Environment.NewLine + ex.Message, Gtk.MessageType.Error, Gtk.ButtonsType.Ok); }
        }

        public static void Save()
        {
            try
            {
                using (FileStream stream = new FileStream(SettingsPath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    HelperClass hc = new HelperClass();
                    bin.Serialize(stream, hc);
                }
            }
            catch (Exception ex) { MessageBox.Show("Couldn´t write Settings!" + Environment.NewLine + ex.Message, Gtk.MessageType.Error, Gtk.ButtonsType.Ok); }
        }

        private static void SetDefaultValues()
        {
            ProgramPath = String.Empty;
            RT = String.Empty;
            LR = String.Empty;

            Threads = 1;
            Program = 0;
            JPGQuality = 100;
            SavingFormat = 0;
            BitDepthPNG = 0;
            BitDepthTiff = 0;

            KeepPP = false;
            TiffCompress = false;
            AutoThreads = false;
        }

        [Serializable()]
        private class HelperClass
        {
            private string HCProgramPath { get; set; }
            private string HCRT { get; set; }
            private string HCLR { get; set; }
            private string HCLastProjDir { get; set; }
            private string HCLastPPDir { get; set; }
            private string HCLastPicDir { get; set; }
            private string HCLastFilterDir { get; set; }
            private string HCLastSaveDir { get; set; }
            private int HCThreads { get; set; }
            private int HCProgram { get; set; }
            private int HCJPGQuality { get; set; }
            private int HCSavingFormat { get; set; }
            private int HCBitDepthPNG { get; set; }
            private int HCBitDepthTiff { get; set; }
            private bool HCKeepPP { get; set; }
            private bool HCTiffCompress { get; set; }
            private bool HCAutoThreads { get; set; }

            public HelperClass()
            {
                HCProgramPath = MySettings.ProgramPath;
                HCRT = MySettings.RT;
                HCLR = MySettings.LR;
                HCLastProjDir = MySettings.LastProjDir;
                HCLastPPDir = MySettings.LastPPDir;
                HCLastPicDir = MySettings.LastPicDir;
                HCLastFilterDir = MySettings.LastFilterDir;
                HCLastSaveDir = MySettings.LastSaveDir;
                HCThreads = MySettings.Threads;
                HCProgram = MySettings.Program;
                HCJPGQuality = MySettings.JPGQuality;
                HCSavingFormat = MySettings.SavingFormat;
                HCBitDepthPNG = MySettings.BitDepthPNG;
                HCBitDepthTiff = MySettings.BitDepthTiff;
                HCKeepPP = MySettings.KeepPP;
                HCTiffCompress = MySettings.TiffCompress;
                HCAutoThreads = MySettings.AutoThreads;
            }

            public void PushValues()
            {
                MySettings.ProgramPath = HCProgramPath;
                MySettings.RT = HCRT;
                MySettings.LR = HCLR;
                MySettings.LastProjDir = HCLastProjDir;
                MySettings.LastPPDir = HCLastPPDir;
                MySettings.LastPicDir = HCLastPicDir;
                MySettings.LastFilterDir = HCLastFilterDir;
                MySettings.LastSaveDir = HCLastSaveDir;
                MySettings.Threads = HCThreads;
                MySettings.Program = HCProgram;
                MySettings.JPGQuality = HCJPGQuality;
                MySettings.SavingFormat = HCSavingFormat;
                MySettings.BitDepthPNG = HCBitDepthPNG;
                MySettings.BitDepthTiff = HCBitDepthTiff;
                MySettings.KeepPP = HCKeepPP;
                MySettings.TiffCompress = HCTiffCompress;
                MySettings.AutoThreads = HCAutoThreads;
            }
        }
    }
}