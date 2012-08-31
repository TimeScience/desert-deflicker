using System;

namespace DeSERt
{
    [Serializable()]
    public class FileData
    {
        #region Variables

        public double Brightness { get; set; }
        public double AltBrightness { get; set; }
        public double Av { get; set; }
        public double Tv { get; set; }
        public double Sv { get; set; }
        public double Bv { get; set; }
        public double WB { get; set; }
        public double StatisticalError { get; set; }

        public string FilePath { get; set; }
        public string Filename { get; set; }
        public string AvString { get; set; }
        public string TvString { get; set; }
        public string SvString { get; set; }
        public string ColorSpace { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }

        public bool IsKeyframe { get; set; }
        public bool HasExif { get; set; }

        public PP3Values PP3 { get; set; }
        public Filterset Filter { get; set; }

        #endregion

        public FileData(string path)
        {
            FilePath = path;
            Filename = System.IO.Path.GetFileName(path);
            ColorSpace = "sRGB";
        }
    }
}
