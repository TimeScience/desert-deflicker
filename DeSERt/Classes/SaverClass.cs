using System;
using System.Collections.Generic;
using Gtk;

namespace DeSERt
{
    [Serializable()]
    public class SaverClass
    {
        public string Version { get; set; }
        public int Program { get; set; }
        public double BrScaleVal { get; set; }
        public double PrevIndexSpinVal { get; set; }
        public double PrevCountSpintVal { get; set; }
        public List<FileData> AllFileData { get; set; }
        public GCurves AllCurveData { get; set; }
        public PP3Values MainPP3 { get; set; }
        public KeyValuePair<int, bool> MovePoint { get; set; }
        public string Workpath { get; set; }
        public bool BrightnessCalculated { get; set; }
        public bool PPfileOpened { get; set; }
    }
}
