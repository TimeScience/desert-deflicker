using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DeSERt
{
    [Serializable()]
    public class PP3Values
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public double NewCompensation { get; set; }

        public VersionStruct Version;
        public GeneralStruct General;
        public ExposureStruct Exposure;
        public ChannelMixerStruct ChannelMixer;
        public LuminanceCurveStruct LuminanceCurve;
        public SharpeningStruct Sharpening;
        public VibranceStruct Vibrance;
        public SharpenEdgeStruct SharpenEdge;
        public SharpenMicroStruct SharpenMicro;
        public WhiteBalanceStruct WhiteBalance;
        public ImpulseDenoisingStruct ImpulseDenoising;
        public DefringingStruct Defringing;
        public DirectionalPyramidDenoisingStruct DirectionalPyramidDenoising;
        public EPDStruct EPD;
        public ShadowsAndHighlightsStruct ShadowsAndHighlights;
        public CropStruct Crop;
        public CoarseTransformationStruct CoarseTransformation;
        public CommonPropertiesForTransformationsStruct CommonPropertiesForTransformations;
        public RotationStruct Rotation;
        public DistortionStruct Distortion;
        public LensProfileStruct LensProfile;
        public PerspectiveStruct Perspective;
        public CACorrectionStruct CACorrection;
        public VignettingCorrectionStruct VignettingCorrection;
        public HLRecoveryStruct HLRecovery;
        public ResizeStruct Resize;
        public ColorManagementStruct ColorManagement;
        public DirectionalPyramidEqualizerStruct DirectionalPyramidEqualizer;
        public HSVEqualizerStruct HSVEqualizer;
        public RGBCurvesStruct RGBCurves;
        public RAWStruct RAW;

        #region Enumerator

        public enum CurveType
        {
            Linear,
            Custom,
            Parametric,
            ControlCage,
            MinMaxControlPoints,
        }
        
        #endregion

        #region Constructor

        public PP3Values()
        {
            Name = "Unknown";
            this.ResetToDefault();
        }

        public PP3Values(string PathToPP3)
        {
            Path = PathToPP3;
            Name = System.IO.Path.GetFileNameWithoutExtension(PathToPP3);
            this.ResetToDefault();
            this.ReadFile(PathToPP3);
        }
        
        #endregion

        #region Substructs

        [Serializable()]
        public struct VersionStruct
        {
            public string AppVersion { get; set; }
            public int Version { get; set; }
        }
        [Serializable()]
        public struct GeneralStruct
        {
            public int Rank { get; set; }
            public int ColorLabel { get; set; }
            public bool InTrash { get; set; }
        }
        [Serializable()]
        public struct ExposureStruct
        {
            public bool Auto { get; set; }
            public double Clip { get; set; }
            public double Compensation { get; set; }
            public int Brightness { get; set; }
            public int Contrast { get; set; }
            public int Saturation { get; set; }
            public int Black { get; set; }
            public int HighlightCompr { get; set; }
            public int HighlightComprThreshold { get; set; }
            public int ShadowCompr { get; set; }
            public PP3Curve Curve { get; set; }
        }
        [Serializable()]
        public struct ChannelMixerStruct
        {
            public int[] Red { get; set; }
            public int[] Green { get; set; }
            public int[] Blue { get; set; }
        }
        [Serializable()]
        public struct LuminanceCurveStruct
        {
            public int Brightness { get; set; }
            public int Contrast { get; set; }
            public int Saturation { get; set; }
            public bool AvoidColorClipping { get; set; }
            public bool SaturationLimiter { get; set; }
            public int SaturationLimit { get; set; }
            public PP3Curve LCurve { get; set; }
            public PP3Curve aCurve { get; set; }
            public PP3Curve bCurve { get; set; }
        }
        [Serializable()]
        public struct SharpeningStruct
        {
            public bool Enabled { get; set; }
            public string Method { get; set; }
            public double Radius { get; set; }
            public int Amount { get; set; }
            public int[] Threshold { get; set; }
            public bool OnlyEdges { get; set; }
            public double EdgedetectionRadius { get; set; }
            public int EdgeTolerance { get; set; }
            public bool HalocontrolEnabled { get; set; }
            public int HalocontrolAmount { get; set; }
            public double DeconvRadius { get; set; }
            public int DeconvAmount { get; set; }
            public int DeconvDamping { get; set; }
            public int DeconvIterations { get; set; }
        }
        [Serializable()]
        public struct VibranceStruct
        {
            public bool Enabled { get; set; }
            public int Pastels { get; set; }
            public int Saturated { get; set; }
            public int[] PSThreshold { get; set; }
            public bool ProtectSkins { get; set; }
            public bool AvoidColorShift { get; set; }
            public bool PastSatTog { get; set; }
        }
        [Serializable()]
        public struct SharpenEdgeStruct
        {
            public bool Enabled { get; set; }
            public int Passes { get; set; }
            public double Strength { get; set; }
            public bool ThreeChannels { get; set; }
        }
        [Serializable()]
        public struct SharpenMicroStruct
        {
            public bool Enabled { get; set; }
            public bool Matrix { get; set; }
            public double Strength { get; set; }
            public double Uniformity { get; set; }
        }
        [Serializable()]
        public struct WhiteBalanceStruct
        {
            public string Setting { get; set; }
            public int Temperature { get; set; }
            public double Green { get; set; }
        }
        [Serializable()]
        public struct ImpulseDenoisingStruct
        {
            public bool Enabled { get; set; }
            public int Threshold { get; set; }
        }
        [Serializable()]
        public struct DefringingStruct
        {
            public bool Enabled { get; set; }
            public double Radius { get; set; }
            public int Threshold { get; set; }
        }
        [Serializable()]
        public struct DirectionalPyramidDenoisingStruct
        {
            public bool Enabled { get; set; }
            public int Luma { get; set; }
            public int Chroma { get; set; }
            public double Gamma { get; set; }
        }
        [Serializable()]
        public struct EPDStruct
        {
            public bool Enabled { get; set; }
            public double Strength { get; set; }
            public double EdgeStopping { get; set; }
            public double Scale { get; set; }
            public int ReweightingIterates { get; set; }
        }
        [Serializable()]
        public struct ShadowsAndHighlightsStruct
        {
            public bool Enabled { get; set; }
            public bool HighQuality { get; set; }
            public int Highlights { get; set; }
            public int HighlightTonalWidth { get; set; }
            public int Shadows { get; set; }
            public int ShadowTonalWidth { get; set; }
            public int LocalContrast { get; set; }
            public int Radius { get; set; }
        }
        [Serializable()]
        public struct CropStruct
        {
            public bool Enabled { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public bool FixedRatio { get; set; }
            public string Ratio { get; set; }
            public string Orientation { get; set; }
            public string Guide { get; set; }
        }
        [Serializable()]
        public struct CoarseTransformationStruct
        {
            public int Rotate { get; set; }
            public bool HorizontalFlip { get; set; }
            public bool VerticalFlip { get; set; }
        }
        [Serializable()]
        public struct CommonPropertiesForTransformationsStruct
        {
            public bool AutoFill { get; set; }
        }
        [Serializable()]
        public struct RotationStruct
        {
            public double Degree { get; set; }
        }
        [Serializable()]
        public struct DistortionStruct
        {
            public double Amount { get; set; }
        }
        [Serializable()]
        public struct LensProfileStruct
        {
            public string LCPFile { get; set; }
            public bool UseDistortion { get; set; }
            public bool UseVignette { get; set; }
            public bool UseCA { get; set; }
        }
        [Serializable()]
        public struct PerspectiveStruct
        {
            public int Horizontal { get; set; }
            public int Vertical { get; set; }
        }
        [Serializable()]
        public struct CACorrectionStruct
        {
            public double Red { get; set; }
            public double Blue { get; set; }
        }
        [Serializable()]
        public struct VignettingCorrectionStruct
        {
            public int Amount { get; set; }
            public int Radius { get; set; }
            public int Strength { get; set; }
            public int CenterX { get; set; }
            public int CenterY { get; set; }
        }
        [Serializable()]
        public struct HLRecoveryStruct
        {
            public bool Enabled { get; set; }
            public string Method { get; set; }
        }
        [Serializable()]
        public struct ResizeStruct
        {
            public bool Enabled { get; set; }
            public double Scale { get; set; }
            public string AppliesTo { get; set; }
            public string Method { get; set; }
            public int DataSpecified { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
        [Serializable()]
        public struct ColorManagementStruct
        {
            public string InputProfile { get; set; }
            public bool BlendCMSMatrix { get; set; }
            public bool PreferredProfile { get; set; }
            public string WorkingProfile { get; set; }
            public string OutputProfile { get; set; }
            public string Gammafree { get; set; }
            public bool Freegamma { get; set; }
            public double GammaValue { get; set; }
            public double GammaSlope { get; set; }
        }
        [Serializable()]
        public struct DirectionalPyramidEqualizerStruct
        {
            public bool Enabled { get; set; }
            public double Mult0 { get; set; }
            public double Mult1 { get; set; }
            public double Mult2 { get; set; }
            public double Mult3 { get; set; }
            public double Mult4 { get; set; }
        }
        [Serializable()]
        public struct HSVEqualizerStruct
        {
            public PP3MinMaxCurve HCurve { get; set; }
            public PP3MinMaxCurve SCurve { get; set; }
            public PP3MinMaxCurve VCurve { get; set; }
        }
        [Serializable()]
        public struct RGBCurvesStruct
        {
            public PP3Curve rCurve { get; set; }
            public PP3Curve gCurve { get; set; }
            public PP3Curve bCurve { get; set; }
        }
        [Serializable()]
        public struct RAWStruct
        {
            public string DarkFrame { get; set; }
            public bool DarkFrameAuto { get; set; }
            public string FlatFieldFile { get; set; }
            public bool FlatFieldAutoSelect { get; set; }
            public int FlatFieldBlurRadius { get; set; }
            public string FlatFieldBlurType { get; set; }
            public bool CA { get; set; }
            public double CARed { get; set; }
            public double CABlue { get; set; }
            public bool HotDeadPixels { get; set; }
            public int HotDeadPixelThresh { get; set; }
            public int LineDenoise { get; set; }
            public int GreenEqThreshold { get; set; }
            public int CcSteps { get; set; }
            public string Method { get; set; }
            public int DCBIterations { get; set; }
            public bool DCBEnhance { get; set; }
            public bool ALLEnhance { get; set; }
            public double PreExposure { get; set; }
            public double PrePreserv { get; set; }
            public double PreBlackzero { get; set; }
            public double PreBlackone { get; set; }
            public double PreBlacktwo { get; set; }
            public double PreBlackthree { get; set; }
            public bool PreTwoGreen { get; set; }
        }

        #endregion

        #region Helperclasses

        [Serializable()]
        public class PP3Curve
        {
            public CurveType Type { get { return Ctype; } }
            public double[] Points { get { return Cpoints; } }
            
            private CurveType Ctype;
            private double[] Cpoints;

            public PP3Curve(double[] values)
            {
                switch ((int)values[0])
                {
                    case 0:
                        Ctype = CurveType.Linear;
                        Cpoints = new double[0];
                        break;
                    case 1:
                        Ctype = CurveType.Custom;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = values[i]; }
                        break;
                    case 2:
                        Ctype = CurveType.Parametric;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = values[i]; }
                        break;
                    case 3:
                        Ctype = CurveType.ControlCage;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = values[i]; }
                        break;
                }
            }

            public PP3Curve(string[] values)
            {
                int curve = Convert.ToInt32(values[0]);
                CultureInfo culture = new CultureInfo("en-US");

                switch (curve)
                {
                    case 0:
                        Ctype = CurveType.Linear;
                        Cpoints = new double[0];
                        break;
                    case 1:
                        Ctype = CurveType.Custom;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = Convert.ToDouble(values[i], culture); }
                        break;
                    case 2:
                        Ctype = CurveType.Parametric;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = Convert.ToDouble(values[i], culture); }
                        break;
                    case 3:
                        Ctype = CurveType.ControlCage;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = Convert.ToDouble(values[i], culture); }
                        break;
                }
            }

            public void SetPoints(double[] NewPoints)
            {
                Cpoints = new double[NewPoints.Length];
                NewPoints.CopyTo(Cpoints, 0);
            }

            public string ToFileString(CultureInfo culture)
            {
                string output = (int)Type + ";";
                for (int i = 0; i < Cpoints.Length; i++)
                {
                    output += Cpoints[i].ToString("N16", culture) + ";";
                }
                return output;
            }
        }

        [Serializable()]
        public class PP3MinMaxCurve
        {
            public CurveType Type { get { return Ctype; } }
            public double[] Points { get { return Cpoints; } }

            private CurveType Ctype;
            private double[] Cpoints;
            
            public PP3MinMaxCurve(double[] values)
            {
                switch ((int)values[0])
                {
                    case 0:
                        Ctype = CurveType.Linear;
                        Cpoints = new double[0];
                        break;
                    case 1:
                        Ctype = CurveType.MinMaxControlPoints;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = values[i]; }
                        break;
                }
            }

            public PP3MinMaxCurve(string[] values)
            {
                CultureInfo culture = new CultureInfo("en-US");
                int curve = Convert.ToInt32(values[0]);
                
                switch (curve)
                {
                    case 0:
                        Ctype = CurveType.Linear;
                        Cpoints = new double[0];
                        break;
                    case 1:
                        Ctype = CurveType.MinMaxControlPoints;
                        Cpoints = new double[values.Length - 1];
                        for (int i = 1; i < values.Length; i++) { Cpoints[i - 1] = Convert.ToDouble(values[i], culture); }
                        break;
                }
            }

            public void SetPoints(double[] NewPoints)
            {
                NewPoints.CopyTo(Cpoints, 0);
            }

            public string ToFileString(CultureInfo culture)
            {
                string output = (int)Type + ";";
                for (int i = 0; i < Cpoints.Length; i++)
                {
                    output += Cpoints[i].ToString("N16", culture) + ";";
                }
                return output;
            }
        }

        #endregion

        #region Subroutines

        public void ReadFile(string PPFilePath)
        {
            Path = PPFilePath;

            string[] lines = File.ReadAllText(PPFilePath).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            CultureInfo culture = new CultureInfo("en-US");
            
            for (int i = 0; i < lines.Length; i++)
            {
                switch (lines[i].ToLower())
                {
                    case "[version]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "appversion":
                                    Version.AppVersion = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "version":
                                    Version.Version = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[general]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "rank":
                                    General.Rank = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "colorlabel":
                                    General.ColorLabel = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "intrash":
                                    General.InTrash = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[exposure]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "auto":
                                    Exposure.Auto = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "clip":
                                    Exposure.Clip = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "compensation":
                                    Exposure.Compensation = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    NewCompensation = Exposure.Compensation;
                                    break;
                                case "brightness":
                                    Exposure.Brightness = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "contrast":
                                    Exposure.Contrast = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "saturation":
                                    Exposure.Saturation = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "black":
                                    Exposure.Black = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "highlightcompr":
                                    Exposure.HighlightCompr = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "highlightcomprthreshold":
                                    Exposure.HighlightComprThreshold = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "shadowcompr":
                                    Exposure.ShadowCompr = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "curve":
                                    Exposure.Curve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                            }
                        }
                        break;

                    case "[channel mixer]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            string[] tmp;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "red":
                                    tmp = lines[i].Substring(lines[i].IndexOf("=") + 1).Split(';');
                                    ChannelMixer.Red = new int[3] { Convert.ToInt32(Convert.ToDouble(tmp[0], culture)), Convert.ToInt32(Convert.ToDouble(tmp[1], culture)), Convert.ToInt32(Convert.ToDouble(tmp[2], culture)) };
                                    break;
                                case "green":
                                    tmp = lines[i].Substring(lines[i].IndexOf("=") + 1).Split(';');
                                    ChannelMixer.Green = new int[3] { Convert.ToInt32(Convert.ToDouble(tmp[0], culture)), Convert.ToInt32(Convert.ToDouble(tmp[1], culture)), Convert.ToInt32(Convert.ToDouble(tmp[2], culture)) };
                                    break;
                                case "blue":
                                    tmp = lines[i].Substring(lines[i].IndexOf("=") + 1).Split(';');
                                    ChannelMixer.Blue = new int[3] { Convert.ToInt32(Convert.ToDouble(tmp[0], culture)), Convert.ToInt32(Convert.ToDouble(tmp[1], culture)), Convert.ToInt32(Convert.ToDouble(tmp[2], culture)) };
                                    break;
                            }
                        }
                        break;

                    case "[luminance curve]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "brightness":
                                    LuminanceCurve.Brightness = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "contrast":
                                    LuminanceCurve.Contrast = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "saturation":
                                    LuminanceCurve.Saturation = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "avoidcolorclipping":
                                    LuminanceCurve.AvoidColorClipping = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "saturationlimiter":
                                    LuminanceCurve.SaturationLimiter = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "saturationlimit":
                                    LuminanceCurve.SaturationLimit = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "lcurve":
                                    LuminanceCurve.LCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "acurve":
                                    LuminanceCurve.aCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "bcurve":
                                    LuminanceCurve.bCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                            }
                        }
                        break;

                    case "[sharpening]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            string[] tmp;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    Sharpening.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "method":
                                    Sharpening.Method = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "radius":
                                    Sharpening.Radius = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "amount":
                                    Sharpening.Amount = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "threshold":
                                    if (lines[i].Contains(";"))
                                    {
                                        tmp = lines[i].Substring(lines[i].IndexOf("=") + 1).Split(';');
                                        Sharpening.Threshold = new int[] { Convert.ToInt32(Convert.ToDouble(tmp[0], culture)), Convert.ToInt32(Convert.ToDouble(tmp[1], culture)), Convert.ToInt32(Convert.ToDouble(tmp[2], culture)), Convert.ToInt32(Convert.ToDouble(tmp[3], culture)) };
                                    }
                                    else { Sharpening.Threshold = new int[] { Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture)) }; }
                                    break;
                                case "onlyedges":
                                    Sharpening.OnlyEdges = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "edgedetectionradius":
                                    Sharpening.EdgedetectionRadius = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "edgetolerance":
                                    Sharpening.EdgeTolerance = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "halocontrolenabled":
                                    Sharpening.HalocontrolEnabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "halocontrolamount":
                                    Sharpening.HalocontrolAmount = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "deconvradius":
                                    Sharpening.DeconvRadius = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "deconvamount":
                                    Sharpening.DeconvAmount = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "deconvdamping":
                                    Sharpening.DeconvDamping = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "deconviterations":
                                    Sharpening.DeconvIterations = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[vibrance]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            string[] tmp;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    Vibrance.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "pastels":
                                    Vibrance.Pastels = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "saturated":
                                    Vibrance.Saturated = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "psthreshold":
                                    if (lines[i].Contains(";"))
                                    {
                                        tmp = lines[i].Substring(lines[i].IndexOf("=") + 1).Split(';');
                                        Vibrance.PSThreshold = new int[] { Convert.ToInt32(Convert.ToDouble(tmp[0], culture)), Convert.ToInt32(Convert.ToDouble(tmp[1], culture)) };
                                    }
                                    else { Vibrance.PSThreshold = new int[] { Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture)) }; }
                                    break;
                                case "protectskins":
                                    Vibrance.ProtectSkins = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "avoidcolorshift":
                                    Vibrance.AvoidColorShift = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "pastsattog":
                                    Vibrance.PastSatTog = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[sharpenedge]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    SharpenEdge.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "passes":
                                    SharpenEdge.Passes = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "strength":
                                    SharpenEdge.Strength = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "threechannels":
                                    SharpenEdge.ThreeChannels = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[sharpenmicro]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    SharpenMicro.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "matrix":
                                    SharpenMicro.Matrix = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "strength":
                                    SharpenMicro.Strength = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "uniformity":
                                    SharpenMicro.Uniformity = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[white balance]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "setting":
                                    WhiteBalance.Setting = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "temperature":
                                    WhiteBalance.Temperature = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "green":
                                    WhiteBalance.Green = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[impulse denoising]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    ImpulseDenoising.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "threshold":
                                    ImpulseDenoising.Threshold = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[defringing]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    Defringing.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "radius":
                                    Defringing.Radius = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "threshold":
                                    Defringing.Threshold = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[directional pyramid denoising]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    DirectionalPyramidDenoising.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "luma":
                                    DirectionalPyramidDenoising.Luma = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "chroma":
                                    DirectionalPyramidDenoising.Chroma = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "gamma":
                                    DirectionalPyramidDenoising.Gamma = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[epd]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    EPD.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "strength":
                                    EPD.Strength = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "edgestopping":
                                    EPD.EdgeStopping = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "scale":
                                    EPD.Scale = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "reweightingiterates":
                                    EPD.ReweightingIterates = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[shadows & highlights]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    ShadowsAndHighlights.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "highquality":
                                    ShadowsAndHighlights.HighQuality = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "highlights":
                                    ShadowsAndHighlights.Highlights = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "highlighttonalwidth":
                                    ShadowsAndHighlights.HighlightTonalWidth = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "shadows":
                                    ShadowsAndHighlights.Shadows = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "shadowtonalwidth":
                                    ShadowsAndHighlights.ShadowTonalWidth = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "localcontrast":
                                    ShadowsAndHighlights.LocalContrast = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "radius":
                                    ShadowsAndHighlights.Radius = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[crop]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    Crop.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "x":
                                    Crop.X = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "y":
                                    Crop.Y = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "w":
                                    Crop.W = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "h":
                                    Crop.H = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "fixedratio":
                                    Crop.FixedRatio = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "ratio":
                                    Crop.Ratio = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "orientation":
                                    Crop.Orientation = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "guide":
                                    Crop.Guide = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[coarse transformation]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "rotate":
                                    CoarseTransformation.Rotate = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "horizontalflip":
                                    CoarseTransformation.HorizontalFlip = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "verticalflip":
                                    CoarseTransformation.VerticalFlip = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[common properties for transformations]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "autofill":
                                    CommonPropertiesForTransformations.AutoFill = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[rotation]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "degree":
                                    Rotation.Degree = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[distortion]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "amount":
                                    Distortion.Amount = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[lensprofile]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "lcpfile":
                                    LensProfile.LCPFile = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "usedistortion":
                                    LensProfile.UseDistortion = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "usevignette":
                                    LensProfile.UseVignette = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "useca":
                                    LensProfile.UseCA = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[perspective]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "horizontal":
                                    Perspective.Horizontal = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "vertical":
                                    Perspective.Vertical = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[cacorrection]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "red":
                                    CACorrection.Red = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "blue":
                                    CACorrection.Blue = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[vignetting correction]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "amount":
                                    VignettingCorrection.Amount = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "radius":
                                    VignettingCorrection.Radius = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "strength":
                                    VignettingCorrection.Strength = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "centerx":
                                    VignettingCorrection.CenterX = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "centery":
                                    VignettingCorrection.CenterY = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[hlrecovery]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    HLRecovery.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "method":
                                    HLRecovery.Method = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                            }
                        }
                        break;

                    case "[resize]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    Resize.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "scale":
                                    Resize.Scale = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "appliesto":
                                    Resize.AppliesTo = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "method":
                                    Resize.Method = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "dataspecified":
                                    Resize.DataSpecified = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "width":
                                    Resize.Width = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "height":
                                    Resize.Height = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                            }
                        }
                        break;

                    case "[color management]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "inputprofile":
                                    ColorManagement.InputProfile = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "blendcmsmatrix":
                                    ColorManagement.BlendCMSMatrix = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "workingprofile":
                                    ColorManagement.WorkingProfile = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "outputprofile":
                                    ColorManagement.OutputProfile = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "gammafree":
                                    ColorManagement.Gammafree = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "freegamma":
                                    ColorManagement.Freegamma = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "gammavalue":
                                    ColorManagement.GammaValue = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "gammaslope":
                                    ColorManagement.GammaSlope = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[directional pyramid equalizer]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "enabled":
                                    DirectionalPyramidEqualizer.Enabled = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "mult0":
                                    DirectionalPyramidEqualizer.Mult0 = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "mult1":
                                    DirectionalPyramidEqualizer.Mult1 = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "mult2":
                                    DirectionalPyramidEqualizer.Mult2 = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "mult3":
                                    DirectionalPyramidEqualizer.Mult3 = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "mult4":
                                    DirectionalPyramidEqualizer.Mult4 = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                            }
                        }
                        break;

                    case "[hsv equalizer]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "hcurve":
                                    HSVEqualizer.HCurve = new PP3MinMaxCurve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "scurve":
                                    HSVEqualizer.SCurve = new PP3MinMaxCurve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "vcurve":
                                    HSVEqualizer.VCurve = new PP3MinMaxCurve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                            }
                        }
                        break;

                    case "[rgb curves]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "rcurve":
                                    RGBCurves.rCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "gcurve":
                                    RGBCurves.gCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                                case "bcurve":
                                    RGBCurves.bCurve = new PP3Curve(lines[i].Substring(lines[i].IndexOf("=") + 1).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                                    break;
                            }
                        }
                        break;
                    case "[raw]":
                        while (!lines[i + 1].StartsWith("["))
                        {
                            i++;
                            switch (lines[i].Substring(0, lines[i].IndexOf("=")).ToLower())
                            {
                                case "darkframe":
                                    RAW.DarkFrame = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "darkframeauto":
                                    RAW.DarkFrameAuto = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "flatfieldfile":
                                    RAW.FlatFieldFile = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "flatfieldautoselect":
                                    RAW.FlatFieldAutoSelect = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "flatfieldblurradius":
                                    RAW.FlatFieldBlurRadius = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "flatfieldblurtype":
                                    RAW.FlatFieldBlurType = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "ca":
                                    RAW.CA = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "cared":
                                    RAW.CARed = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "cablue":
                                    RAW.CABlue = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "hotdeadpixels":
                                    RAW.HotDeadPixels = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "hotdeadpixelthresh":
                                    RAW.HotDeadPixelThresh = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "linedenoise":
                                    RAW.LineDenoise = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "greeneqthreshold":
                                    RAW.GreenEqThreshold = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "ccsteps":
                                    RAW.CcSteps = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "method":
                                    RAW.Method = Convert.ToString(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "dcbiterations":
                                    RAW.DCBIterations = Convert.ToInt32(Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture));
                                    break;
                                case "dcbenhance":
                                    RAW.DCBEnhance = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "allenhance":
                                    RAW.ALLEnhance = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    break;
                                case "preexposure":
                                    RAW.PreExposure = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "prepreserv":
                                    RAW.PrePreserv = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "preblackzero":
                                    RAW.PreBlackzero = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "preblackone":
                                    RAW.PreBlackone = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "preblacktwo":
                                    RAW.PreBlacktwo = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "preblackthree":
                                    RAW.PreBlackthree = Convert.ToDouble(lines[i].Substring(lines[i].IndexOf("=") + 1), culture);
                                    break;
                                case "pretwogreen":
                                    RAW.PreTwoGreen = Convert.ToBoolean(lines[i].Substring(lines[i].IndexOf("=") + 1));
                                    return;
                            }
                        }
                        break;
                }
            }
        }

        public void WriteFile(string Path)
        {
            StreamWriter writer = new StreamWriter(Path);
            CultureInfo culture = new CultureInfo("en-US");

            writer.WriteLine();
            writer.WriteLine("[Version]");
            writer.WriteLine("AppVersion=" + Version.AppVersion);
            writer.WriteLine("Version=" + Version.Version);
            writer.WriteLine();
            writer.WriteLine("[General]");
            writer.WriteLine("Rank=" + General.Rank);
            writer.WriteLine("ColorLabel=" + General.ColorLabel);
            writer.WriteLine("InTrash=" + General.InTrash.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[Exposure]");
            writer.WriteLine("Auto=" + Exposure.Auto.ToString().ToLower());
            writer.WriteLine("Clip=" + Exposure.Clip.ToString("N16", culture));
            writer.WriteLine("Compensation=" + NewCompensation.ToString("N16", culture));
            writer.WriteLine("Brightness=" + Exposure.Brightness);
            writer.WriteLine("Contrast=" + Exposure.Contrast);
            writer.WriteLine("Saturation=" + Exposure.Saturation);
            writer.WriteLine("Black=" + Exposure.Black);
            writer.WriteLine("HighlightCompr=" + Exposure.HighlightCompr);
            writer.WriteLine("HighlightComprThreshold=" + Exposure.HighlightComprThreshold);
            writer.WriteLine("ShadowCompr=" + Exposure.ShadowCompr);
            writer.WriteLine("Curve=" + Exposure.Curve.ToFileString(culture));
            writer.WriteLine();
            writer.WriteLine("[Channel Mixer]");
            writer.WriteLine("Red=" + ChannelMixer.Red[0] + ";" + ChannelMixer.Red[1] + ";" +ChannelMixer.Red[2] + ";");
            writer.WriteLine("Green=" + ChannelMixer.Green[0] + ";" + ChannelMixer.Green[1] + ";" + ChannelMixer.Green[2] + ";");
            writer.WriteLine("Blue=" + ChannelMixer.Blue[0] + ";" + ChannelMixer.Blue[1] + ";" + ChannelMixer.Blue[2] + ";");
            writer.WriteLine();
            writer.WriteLine("[Luminance Curve]");
            writer.WriteLine("Brightness=" + LuminanceCurve.Brightness);
            writer.WriteLine("Contrast=" + LuminanceCurve.Contrast);
            writer.WriteLine("Saturation=" + LuminanceCurve.Saturation);
            writer.WriteLine("AvoidColorClipping=" + LuminanceCurve.AvoidColorClipping.ToString().ToLower());
            writer.WriteLine("SaturationLimiter=" + LuminanceCurve.SaturationLimiter.ToString().ToLower());
            writer.WriteLine("SaturationLimit=" + LuminanceCurve.SaturationLimit);
            writer.WriteLine("LCurve=" + LuminanceCurve.LCurve.ToFileString(culture));
            writer.WriteLine("aCurve=" + LuminanceCurve.aCurve.ToFileString(culture));
            writer.WriteLine("bCurve=" + LuminanceCurve.bCurve.ToFileString(culture));
            writer.WriteLine();
            writer.WriteLine("[Sharpening]");
            writer.WriteLine("Enabled=" + Sharpening.Enabled.ToString().ToLower());
            writer.WriteLine("Method=" + Sharpening.Method);
            writer.WriteLine("Radius=" + Sharpening.Radius.ToString("N16", culture));
            writer.WriteLine("Amount=" + Sharpening.Amount);
            if (Sharpening.Threshold.Length == 1) { writer.WriteLine("Threshold=" + Sharpening.Threshold[0]); }
            else { writer.WriteLine("Threshold=" + Sharpening.Threshold[0] + ";" + Sharpening.Threshold[1] + ";" + Sharpening.Threshold[2] + ";" + Sharpening.Threshold[3] + ";"); }
            writer.WriteLine("OnlyEdges=" + Sharpening.OnlyEdges.ToString().ToLower());
            writer.WriteLine("EdgedetectionRadius=" + Sharpening.EdgedetectionRadius.ToString("N16", culture));
            writer.WriteLine("EdgeTolerance=" + Sharpening.EdgeTolerance);
            writer.WriteLine("HalocontrolEnabled=" + Sharpening.HalocontrolEnabled.ToString().ToLower());
            writer.WriteLine("HalocontrolAmount=" + Sharpening.HalocontrolAmount);
            writer.WriteLine("DeconvRadius=" + Sharpening.DeconvRadius.ToString("N16", culture));
            writer.WriteLine("DeconvAmount=" + Sharpening.DeconvAmount);
            writer.WriteLine("DeconvDamping=" + Sharpening.DeconvDamping);
            writer.WriteLine("DeconvIterations=" + Sharpening.DeconvIterations);
            writer.WriteLine();
            writer.WriteLine("[Vibrance]");
            writer.WriteLine("Enabled=" + Vibrance.Enabled.ToString().ToLower());
            writer.WriteLine("Pastels=" + Vibrance.Pastels);
            writer.WriteLine("Saturated=" + Vibrance.Saturated);
            if (Vibrance.PSThreshold.Length == 1) { writer.WriteLine("Threshold=" + Vibrance.PSThreshold[0]); }
            else { writer.WriteLine("Threshold=" + Vibrance.PSThreshold[0] + ";" + Vibrance.PSThreshold[1] + ";"); }
            writer.WriteLine("ProtectSkins=" + Vibrance.ProtectSkins.ToString().ToLower());
            writer.WriteLine("AvoidColorShift=" + Vibrance.AvoidColorShift.ToString().ToLower());
            writer.WriteLine("PastSatTog=" + Vibrance.PastSatTog.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[SharpenEdge]");
            writer.WriteLine("Enabled=" + SharpenEdge.Enabled.ToString().ToLower());
            writer.WriteLine("Passes=" + SharpenEdge.Passes);
            writer.WriteLine("Strength=" + SharpenEdge.Strength.ToString("N16", culture));
            writer.WriteLine("ThreeChannels=" + SharpenEdge.ThreeChannels.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[SharpenMicro]");
            writer.WriteLine("Enabled=" + SharpenMicro.Enabled.ToString().ToLower());
            writer.WriteLine("Matrix=" + SharpenMicro.Matrix.ToString().ToLower());
            writer.WriteLine("Strength=" + SharpenMicro.Strength.ToString("N16", culture));
            writer.WriteLine("Uniformity=" + SharpenMicro.Uniformity.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[White Balance]");
            writer.WriteLine("Setting=" + WhiteBalance.Setting);
            writer.WriteLine("Temperature=" + WhiteBalance.Temperature);
            writer.WriteLine("Green=" + WhiteBalance.Green.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[Impulse Denoising]");
            writer.WriteLine("Enabled=" + ImpulseDenoising.Enabled.ToString().ToLower());
            writer.WriteLine("Threshold=" + ImpulseDenoising.Threshold);
            writer.WriteLine();
            writer.WriteLine("[Defringing]");
            writer.WriteLine("Enabled=" + Defringing.Enabled.ToString().ToLower());
            writer.WriteLine("Radius=" + Defringing.Radius.ToString("N16", culture));
            writer.WriteLine("Threshold=" + Defringing.Threshold);
            writer.WriteLine();
            writer.WriteLine("[Directional Pyramid Denoising]");
            writer.WriteLine("Enabled=" + DirectionalPyramidDenoising.Enabled.ToString().ToLower());
            writer.WriteLine("Luma=" + DirectionalPyramidDenoising.Luma);
            writer.WriteLine("Chroma=" + DirectionalPyramidDenoising.Chroma);
            writer.WriteLine("Gamma=" + DirectionalPyramidDenoising.Gamma.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[EPD]");
            writer.WriteLine("Enabled=" + EPD.Enabled.ToString().ToLower());
            writer.WriteLine("Strength=" + EPD.Strength.ToString("N16", culture));
            writer.WriteLine("EdgeStopping=" + EPD.EdgeStopping.ToString("N16", culture));
            writer.WriteLine("Scale=" + EPD.Scale.ToString("N16", culture));
            writer.WriteLine("ReweightingIterates=" + EPD.ReweightingIterates);
            writer.WriteLine();
            writer.WriteLine("[Shadows & Highlights]");
            writer.WriteLine("Enabled=" + ShadowsAndHighlights.Enabled.ToString().ToLower());
            writer.WriteLine("HighQuality=" + ShadowsAndHighlights.HighQuality.ToString().ToLower());
            writer.WriteLine("Highlights=" + ShadowsAndHighlights.Highlights);
            writer.WriteLine("HighlightTonalWidth=" + ShadowsAndHighlights.HighlightTonalWidth);
            writer.WriteLine("Shadows=" + ShadowsAndHighlights.Shadows);
            writer.WriteLine("ShadowTonalWidth=" + ShadowsAndHighlights.ShadowTonalWidth);
            writer.WriteLine("LocalContrast=" + ShadowsAndHighlights.LocalContrast);
            writer.WriteLine("Radius=" + ShadowsAndHighlights.Radius);
            writer.WriteLine();
            writer.WriteLine("[Crop]");
            writer.WriteLine("Enabled=" + Crop.Enabled.ToString().ToLower());
            writer.WriteLine("X=" + Crop.X);
            writer.WriteLine("Y=" + Crop.Y);
            writer.WriteLine("W=" + Crop.W);
            writer.WriteLine("H=" + Crop.H);
            writer.WriteLine("FixedRatio=" + Crop.FixedRatio.ToString().ToLower());
            writer.WriteLine("Ratio=" + Crop.Ratio);
            writer.WriteLine("Orientation=" + Crop.Orientation);
            writer.WriteLine("Guide=" + Crop.Guide);
            writer.WriteLine();
            writer.WriteLine("[Coarse Transformation]");
            writer.WriteLine("Rotate=" + CoarseTransformation.Rotate);
            writer.WriteLine("HorizontalFlip=" + CoarseTransformation.HorizontalFlip.ToString().ToLower());
            writer.WriteLine("VerticalFlip=" + CoarseTransformation.VerticalFlip.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[Common Properties for Transformations]");
            writer.WriteLine("AutoFill=" + CommonPropertiesForTransformations.AutoFill.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[Rotation]");
            writer.WriteLine("Degree=" + Rotation.Degree.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[Distortion]");
            writer.WriteLine("Amount=" + Distortion.Amount.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[LensProfile]");
            writer.WriteLine("LCPFile=" + LensProfile.LCPFile);
            writer.WriteLine("UseDistortion=" + LensProfile.UseDistortion.ToString().ToLower());
            writer.WriteLine("UseVignette=" + LensProfile.UseVignette.ToString().ToLower());
            writer.WriteLine("UseCA=" + LensProfile.UseCA.ToString().ToLower());
            writer.WriteLine();
            writer.WriteLine("[Perspective]");
            writer.WriteLine("Horizontal=" + Perspective.Horizontal);
            writer.WriteLine("Vertical=" + Perspective.Vertical);
            writer.WriteLine();
            writer.WriteLine("[CACorrection]");
            writer.WriteLine("Red=" + CACorrection.Red.ToString("N16", culture));
            writer.WriteLine("Blue=" + CACorrection.Blue.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[Vignetting Correction]");
            writer.WriteLine("Amount=" + VignettingCorrection.Amount);
            writer.WriteLine("Radius=" + VignettingCorrection.Radius);
            writer.WriteLine("Strength=" + VignettingCorrection.Strength);
            writer.WriteLine("CenterX=" + VignettingCorrection.CenterX);
            writer.WriteLine("CenterY=" + VignettingCorrection.CenterY);
            writer.WriteLine();
            writer.WriteLine("[HLRecovery]");
            writer.WriteLine("Enabled=" + HLRecovery.Enabled.ToString().ToLower());
            writer.WriteLine("Method=" + HLRecovery.Method);
            writer.WriteLine();
            writer.WriteLine("[Resize]");
            writer.WriteLine("Enabled=" + Resize.Enabled.ToString().ToLower());
            writer.WriteLine("Scale=" + Resize.Scale.ToString("N16", culture));
            writer.WriteLine("AppliesTo=" + Resize.AppliesTo);
            writer.WriteLine("Method=" + Resize.Method);
            writer.WriteLine("DataSpecified=" + Resize.DataSpecified);
            writer.WriteLine("Width=" + Resize.Width);
            writer.WriteLine("Height=" + Resize.Height);
            writer.WriteLine();
            writer.WriteLine("[Color Management]");
            writer.WriteLine("InputProfile=" + ColorManagement.InputProfile);
            writer.WriteLine("BlendCMSMatrix=" + ColorManagement.BlendCMSMatrix.ToString().ToLower());
            writer.WriteLine("WorkingProfile=" + ColorManagement.WorkingProfile);
            writer.WriteLine("OutputProfile=" + ColorManagement.OutputProfile);
            writer.WriteLine("Gammafree=" + ColorManagement.Gammafree);
            writer.WriteLine("Freegamma=" + ColorManagement.Freegamma.ToString().ToLower());
            writer.WriteLine("GammaValue=" + ColorManagement.GammaValue.ToString("N16", culture));
            writer.WriteLine("GammaSlope=" + ColorManagement.GammaSlope.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[Directional Pyramid Equalizer]");
            writer.WriteLine("Enabled=" + DirectionalPyramidEqualizer.Enabled.ToString().ToLower());
            writer.WriteLine("Mult0=" + DirectionalPyramidEqualizer.Mult0.ToString("N16", culture));
            writer.WriteLine("Mult1=" + DirectionalPyramidEqualizer.Mult1.ToString("N16", culture));
            writer.WriteLine("Mult2=" + DirectionalPyramidEqualizer.Mult2.ToString("N16", culture));
            writer.WriteLine("Mult3=" + DirectionalPyramidEqualizer.Mult3.ToString("N16", culture));
            writer.WriteLine("Mult4=" + DirectionalPyramidEqualizer.Mult4.ToString("N16", culture));
            writer.WriteLine();
            writer.WriteLine("[HSV Equalizer]");
            writer.WriteLine("HCurve=" + HSVEqualizer.HCurve.ToFileString(culture));
            writer.WriteLine("SCurve=" + HSVEqualizer.SCurve.ToFileString(culture));
            writer.WriteLine("VCurve=" + HSVEqualizer.VCurve.ToFileString(culture));
            writer.WriteLine();
            writer.WriteLine("[RGB Curves]");
            writer.WriteLine("rCurve=" + RGBCurves.rCurve.ToFileString(culture));
            writer.WriteLine("gCurve=" + RGBCurves.gCurve.ToFileString(culture));
            writer.WriteLine("bCurve=" + RGBCurves.bCurve.ToFileString(culture));
            writer.WriteLine();
            writer.WriteLine("[RAW]");
            writer.WriteLine("DarkFrame=" + RAW.DarkFrame);
            writer.WriteLine("DarkFrameAuto=" + RAW.DarkFrameAuto.ToString().ToLower());
            writer.WriteLine("FlatFieldFile=" + RAW.FlatFieldFile);
            writer.WriteLine("FlatFieldAutoSelect=" + RAW.FlatFieldAutoSelect.ToString().ToLower());
            writer.WriteLine("FlatFieldBlurRadius=" + RAW.FlatFieldBlurRadius);
            writer.WriteLine("FlatFieldBlurType=" + RAW.FlatFieldBlurType);
            writer.WriteLine("CA=" + RAW.CA.ToString().ToLower());
            writer.WriteLine("CARed=" + RAW.CARed.ToString("N16", culture));
            writer.WriteLine("CABlue=" + RAW.CABlue.ToString("N16", culture));
            writer.WriteLine("HotDeadPixels=" + RAW.HotDeadPixels.ToString().ToLower());
            writer.WriteLine("HotDeadPixelThresh=" + RAW.HotDeadPixelThresh);
            writer.WriteLine("LineDenoise=" + RAW.LineDenoise);
            writer.WriteLine("GreenEqThreshold=" + RAW.GreenEqThreshold);
            writer.WriteLine("CcSteps=" + RAW.CcSteps);
            writer.WriteLine("Method=" + RAW.Method);
            writer.WriteLine("DCBIterations=" + RAW.DCBIterations);
            writer.WriteLine("DCBEnhance=" + RAW.DCBEnhance.ToString().ToLower());
            writer.WriteLine("ALLEnhance=" + RAW.ALLEnhance.ToString().ToLower());
            writer.WriteLine("PreExposure=" + RAW.PreExposure.ToString("N16", culture));
            writer.WriteLine("PrePreserv=" + RAW.PrePreserv.ToString("N16", culture));
            writer.WriteLine("PreBlackzero=" + RAW.PreBlackzero.ToString("N16", culture));
            writer.WriteLine("PreBlackone=" + RAW.PreBlackone.ToString("N16", culture));
            writer.WriteLine("PreBlacktwo=" + RAW.PreBlacktwo.ToString("N16", culture));
            writer.WriteLine("PreBlackthree=" + RAW.PreBlackthree.ToString("N16", culture));
            writer.WriteLine("PreTwoGreen=" + RAW.PreTwoGreen.ToString().ToLower());
            writer.WriteLine();

            writer.Close();
        }

        public void ResetToDefault()
        {
            NewCompensation = 0;

            Version.AppVersion = "4.0.9";
            Version.Version = 301;

            General.Rank = 0;
            General.ColorLabel = 0;
            General.InTrash = false;

            Exposure.Auto = false;
            Exposure.Clip = 0;
            Exposure.Compensation = 0;
            Exposure.Brightness = 0;
            Exposure.Contrast = 0;
            Exposure.Saturation = 0;
            Exposure.Black = 0;
            Exposure.HighlightCompr = 0;
            Exposure.HighlightComprThreshold = 0;
            Exposure.ShadowCompr = 0;
            Exposure.Curve = new PP3Curve(new double[1] { 0 });

            ChannelMixer.Red = new int[3] {100, 0, 0 };
            ChannelMixer.Green = new int[3] { 0, 100, 0 };
            ChannelMixer.Blue = new int[3] { 0, 0, 100 };

            LuminanceCurve.Brightness = 0;
            LuminanceCurve.Contrast = 0;
            LuminanceCurve.Saturation = 0;
            LuminanceCurve.AvoidColorClipping = false;
            LuminanceCurve.SaturationLimiter = false;
            LuminanceCurve.SaturationLimit = 50;
            LuminanceCurve.LCurve = new PP3Curve(new double[1] { 0 });
            LuminanceCurve.aCurve = new PP3Curve(new double[1] { 0 });
            LuminanceCurve.bCurve = new PP3Curve(new double[1] { 0 });

            Sharpening.Enabled = false;
            Sharpening.Method = "usm";
            Sharpening.Radius = 0.5f;
            Sharpening.Amount = 125;
            Sharpening.Threshold = new int[] { 20, 80, 2000, 1200 };
            Sharpening.OnlyEdges = false;
            Sharpening.EdgedetectionRadius = 1.9f;
            Sharpening.EdgeTolerance = 1800;
            Sharpening.HalocontrolEnabled = false;
            Sharpening.HalocontrolAmount = 85;
            Sharpening.DeconvRadius = 0.75f;
            Sharpening.DeconvAmount = 75;
            Sharpening.DeconvDamping = 20;
            Sharpening.DeconvIterations = 30;

            Vibrance.Enabled = false;
            Vibrance.Pastels = 50;
            Vibrance.Saturated = 50;
            Vibrance.PSThreshold = new int[] { 0, 75 };
            Vibrance.ProtectSkins = false;
            Vibrance.AvoidColorShift = true;
            Vibrance.PastSatTog = true;

            SharpenEdge.Enabled = false;
            SharpenEdge.Passes = 2;
            SharpenEdge.Strength = 50;
            SharpenEdge.ThreeChannels = false;

            SharpenMicro.Enabled = false;
            SharpenMicro.Matrix = false;
            SharpenMicro.Strength = 20;
            SharpenMicro.Uniformity = 50;

            WhiteBalance.Setting = "Camera";
            WhiteBalance.Temperature = 5745;
            WhiteBalance.Green = 1;

            ImpulseDenoising.Enabled = false;
            ImpulseDenoising.Threshold = 50;

            Defringing.Enabled = false;
            Defringing.Radius = 2;
            Defringing.Threshold = 25;

            DirectionalPyramidDenoising.Enabled = false;
            DirectionalPyramidDenoising.Luma = 5;
            DirectionalPyramidDenoising.Chroma = 5;
            DirectionalPyramidDenoising.Gamma = 2;

            EPD.Enabled = false;
            EPD.Strength = 0.25f;
            EPD.EdgeStopping = 1.4f;
            EPD.Scale = 1;
            EPD.ReweightingIterates = 0;

            ShadowsAndHighlights.Enabled = false;
            ShadowsAndHighlights.HighQuality = false;
            ShadowsAndHighlights.Highlights = 10;
            ShadowsAndHighlights.HighlightTonalWidth = 80;
            ShadowsAndHighlights.Shadows = 10;
            ShadowsAndHighlights.ShadowTonalWidth = 80;
            ShadowsAndHighlights.LocalContrast = 0;
            ShadowsAndHighlights.Radius = 30;

            Crop.Enabled = false;
            Crop.X = 0;
            Crop.Y = 0;
            Crop.W = 7360;
            Crop.H = 4912;
            Crop.FixedRatio = false;
            Crop.Ratio = "3:2";
            Crop.Orientation = "Landscape";
            Crop.Guide = "None";

            CoarseTransformation.Rotate = 0;
            CoarseTransformation.HorizontalFlip = false;
            CoarseTransformation.VerticalFlip = false;

            CommonPropertiesForTransformations.AutoFill = true;

            Rotation.Degree = 0;

            Distortion.Amount = 0;

            LensProfile.LCPFile = String.Empty;
            LensProfile.UseDistortion = true;
            LensProfile.UseVignette = true;
            LensProfile.UseCA = false;

            Perspective.Horizontal = 0;
            Perspective.Vertical = 0;

            CACorrection.Red = 0;
            CACorrection.Blue = 0;

            VignettingCorrection.Amount = 0;
            VignettingCorrection.Radius = 50;
            VignettingCorrection.Strength = 1;
            VignettingCorrection.CenterX = 0;
            VignettingCorrection.CenterY = 0;

            HLRecovery.Enabled = false;
            HLRecovery.Method = "Blend";

            Resize.Enabled = false;
            Resize.Scale = 1;
            Resize.AppliesTo = "Cropped area";
            Resize.Method = "Lanczos";
            Resize.DataSpecified = 3;
            Resize.Width = 900;
            Resize.Height = 900;

            ColorManagement.InputProfile = "(cameraICC)";
            ColorManagement.BlendCMSMatrix = true;
            ColorManagement.WorkingProfile = "sRGB";
            ColorManagement.OutputProfile = "No ICM: sRGB output";
            ColorManagement.Gammafree = "default";
            ColorManagement.Freegamma = false;
            ColorManagement.GammaValue = 2.22f;
            ColorManagement.GammaSlope = 4.5f;

            DirectionalPyramidEqualizer.Enabled = false;
            DirectionalPyramidEqualizer.Mult0 = 1;
            DirectionalPyramidEqualizer.Mult1 = 1;
            DirectionalPyramidEqualizer.Mult2 = 1;
            DirectionalPyramidEqualizer.Mult3 = 1;
            DirectionalPyramidEqualizer.Mult4 = 0.2;

            HSVEqualizer.HCurve = new PP3MinMaxCurve(new double[1] { 0 });
            HSVEqualizer.SCurve = new PP3MinMaxCurve(new double[1] { 0 });
            HSVEqualizer.VCurve = new PP3MinMaxCurve(new double[1] { 0 });

            RGBCurves.rCurve = new PP3Curve(new double[1] { 0 });
            RGBCurves.gCurve = new PP3Curve(new double[1] { 0 });
            RGBCurves.bCurve = new PP3Curve(new double[1] { 0 });

            RAW.DarkFrame = String.Empty;
            RAW.DarkFrameAuto = false;
            RAW.FlatFieldFile = String.Empty;
            RAW.FlatFieldAutoSelect = false;
            RAW.FlatFieldBlurRadius = 32;
            RAW.FlatFieldBlurType = "Area Flatfield";
            RAW.CA = false;
            RAW.CARed = 0;
            RAW.CABlue = 0;
            RAW.HotDeadPixels = false;
            RAW.HotDeadPixelThresh = 40;
            RAW.LineDenoise = 0;
            RAW.GreenEqThreshold = 0;
            RAW.CcSteps = 0;
            RAW.Method = "amaze";
            RAW.DCBIterations = 2;
            RAW.DCBEnhance = false;
            RAW.ALLEnhance = false;
            RAW.PreExposure = 1;
            RAW.PrePreserv = 0;
            RAW.PreBlackzero = 0;
            RAW.PreBlackone = 0;
            RAW.PreBlacktwo = 0;
            RAW.PreBlackthree = 0;
            RAW.PreTwoGreen = true;
        }

        public List<KeyValuePair<int, double[]>> GetAllCurves()
        {
            List<KeyValuePair<int, double[]>> Output = new List<KeyValuePair<int,double[]>>();

            Output.Add(new KeyValuePair<int, double[]>((int)Exposure.Curve.Type, Exposure.Curve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)LuminanceCurve.LCurve.Type, LuminanceCurve.LCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)LuminanceCurve.aCurve.Type, LuminanceCurve.aCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)LuminanceCurve.bCurve.Type, LuminanceCurve.bCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)HSVEqualizer.HCurve.Type, HSVEqualizer.HCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)HSVEqualizer.SCurve.Type, HSVEqualizer.SCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)HSVEqualizer.VCurve.Type, HSVEqualizer.VCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)RGBCurves.rCurve.Type, RGBCurves.rCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)RGBCurves.gCurve.Type, RGBCurves.gCurve.Points));
            Output.Add(new KeyValuePair<int, double[]>((int)RGBCurves.bCurve.Type, RGBCurves.bCurve.Points));

            double[] tmp = new double[3];
            ChannelMixer.Red.CopyTo(tmp, 0);
            Output.Add(new KeyValuePair<int, double[]>(-1, tmp));
            tmp = new double[3];
            ChannelMixer.Green.CopyTo(tmp, 0);
            Output.Add(new KeyValuePair<int, double[]>(-1, tmp));
            tmp = new double[3];
            ChannelMixer.Blue.CopyTo(tmp, 0);
            Output.Add(new KeyValuePair<int, double[]>(-1, tmp));
            
            return Output;
        }

        public void SetAllCurves(List<double[]> NewCurves)
        {
            Exposure.Curve.SetPoints(NewCurves[0]);
            LuminanceCurve.LCurve.SetPoints(NewCurves[1]);
            LuminanceCurve.aCurve.SetPoints(NewCurves[2]);
            LuminanceCurve.bCurve.SetPoints(NewCurves[3]);
            HSVEqualizer.HCurve.SetPoints(NewCurves[4]);
            HSVEqualizer.SCurve.SetPoints(NewCurves[5]);
            HSVEqualizer.VCurve.SetPoints(NewCurves[6]);
            RGBCurves.rCurve.SetPoints(NewCurves[7]);
            RGBCurves.gCurve.SetPoints(NewCurves[8]);
            RGBCurves.bCurve.SetPoints(NewCurves[9]);

            int[] Red = new int[3];
            int[] Green = new int[3];
            int[] Blue = new int[3];
            for (int i = 0; i < 3; i++)
            {
                Red[i] = Convert.ToInt32(NewCurves[10][i]);
                Green[i] = Convert.ToInt32(NewCurves[11][i]);
                Blue[i] = Convert.ToInt32(NewCurves[12][i]);
            }
            Red.CopyTo(ChannelMixer.Red, 0);
            Green.CopyTo(ChannelMixer.Green, 0);
            Blue.CopyTo(ChannelMixer.Blue, 0);
        }

        public List<double> GetAllInterpolateValues()
        {
            List<double> Allvals = new List<double>();
            
            Allvals.Add(Exposure.Clip);
            Allvals.Add(Exposure.Brightness);
            Allvals.Add(Exposure.Contrast);
            Allvals.Add(Exposure.Saturation);
            Allvals.Add(Exposure.Black);
            Allvals.Add(Exposure.HighlightCompr);
            Allvals.Add(Exposure.HighlightComprThreshold);
            Allvals.Add(Exposure.ShadowCompr);
            Allvals.Add(LuminanceCurve.Brightness);
            Allvals.Add(LuminanceCurve.Contrast);
            Allvals.Add(LuminanceCurve.Saturation);
            Allvals.Add(LuminanceCurve.SaturationLimit);
            Allvals.Add(Sharpening.Radius);
            Allvals.Add(Sharpening.Amount);
            Allvals.Add(Sharpening.EdgedetectionRadius);
            Allvals.Add(Sharpening.EdgeTolerance);
            Allvals.Add(Sharpening.HalocontrolAmount);
            Allvals.Add(Sharpening.DeconvRadius);
            Allvals.Add(Sharpening.DeconvAmount);
            Allvals.Add(Sharpening.DeconvDamping);
            Allvals.Add(Sharpening.DeconvIterations);
            Allvals.Add(Vibrance.Pastels);
            Allvals.Add(Vibrance.Saturated);
            Allvals.Add(SharpenEdge.Passes);
            Allvals.Add(SharpenEdge.Strength);
            Allvals.Add(SharpenMicro.Strength);
            Allvals.Add(SharpenMicro.Uniformity);
            Allvals.Add(WhiteBalance.Temperature);
            Allvals.Add(WhiteBalance.Green);
            Allvals.Add(ImpulseDenoising.Threshold);
            Allvals.Add(Defringing.Radius);
            Allvals.Add(Defringing.Threshold);
            Allvals.Add(DirectionalPyramidDenoising.Luma);
            Allvals.Add(DirectionalPyramidDenoising.Chroma);
            Allvals.Add(DirectionalPyramidDenoising.Gamma);
            Allvals.Add(EPD.Strength);
            Allvals.Add(EPD.EdgeStopping);
            Allvals.Add(EPD.Scale);
            Allvals.Add(EPD.ReweightingIterates);
            Allvals.Add(ShadowsAndHighlights.Highlights);
            Allvals.Add(ShadowsAndHighlights.HighlightTonalWidth);
            Allvals.Add(ShadowsAndHighlights.Shadows);
            Allvals.Add(ShadowsAndHighlights.ShadowTonalWidth);
            Allvals.Add(ShadowsAndHighlights.LocalContrast);
            Allvals.Add(ShadowsAndHighlights.Radius);
            Allvals.Add(Crop.X);
            Allvals.Add(Crop.Y);
            Allvals.Add(Rotation.Degree);
            Allvals.Add(Distortion.Amount);
            Allvals.Add(Perspective.Horizontal);
            Allvals.Add(Perspective.Vertical);
            Allvals.Add(CACorrection.Red);
            Allvals.Add(CACorrection.Blue);
            Allvals.Add(VignettingCorrection.Amount);
            Allvals.Add(VignettingCorrection.Radius);
            Allvals.Add(VignettingCorrection.Strength);
            Allvals.Add(VignettingCorrection.CenterX);
            Allvals.Add(VignettingCorrection.CenterY);
            Allvals.Add(Resize.Scale);
            Allvals.Add(Resize.Width);
            Allvals.Add(Resize.Height);
            Allvals.Add(ColorManagement.GammaValue);
            Allvals.Add(ColorManagement.GammaSlope);
            Allvals.Add(DirectionalPyramidEqualizer.Mult0);
            Allvals.Add(DirectionalPyramidEqualizer.Mult1);
            Allvals.Add(DirectionalPyramidEqualizer.Mult2);
            Allvals.Add(DirectionalPyramidEqualizer.Mult3);
            Allvals.Add(DirectionalPyramidEqualizer.Mult4);
            Allvals.Add(RAW.FlatFieldBlurRadius);
            Allvals.Add(RAW.CARed);
            Allvals.Add(RAW.CABlue);
            Allvals.Add(RAW.LineDenoise);
            Allvals.Add(RAW.GreenEqThreshold);
            Allvals.Add(RAW.CcSteps);
            Allvals.Add(RAW.DCBIterations);
            Allvals.Add(RAW.PreExposure);
            Allvals.Add(RAW.PrePreserv);
            Allvals.Add(RAW.PreBlackzero);
            Allvals.Add(RAW.PreBlackone);
            Allvals.Add(RAW.PreBlacktwo);
            Allvals.Add(RAW.PreBlackthree);

            return Allvals;
        }

        public void SetAllInterpolateValues(List<double> NewVals)
        {
            Exposure.Clip = Convert.ToDouble(NewVals[0]);
            Exposure.Brightness = Convert.ToInt32(NewVals[1]);
            Exposure.Contrast = Convert.ToInt32(NewVals[2]);
            Exposure.Saturation = Convert.ToInt32(NewVals[3]);
            Exposure.Black = Convert.ToInt32(NewVals[4]);
            Exposure.HighlightCompr = Convert.ToInt32(NewVals[5]);
            Exposure.HighlightComprThreshold = Convert.ToInt32(NewVals[6]);
            Exposure.ShadowCompr = Convert.ToInt32(NewVals[7]);
            LuminanceCurve.Brightness = Convert.ToInt32(NewVals[8]);
            LuminanceCurve.Contrast = Convert.ToInt32(NewVals[9]);
            LuminanceCurve.Saturation = Convert.ToInt32(NewVals[10]);
            LuminanceCurve.SaturationLimit = Convert.ToInt32(NewVals[11]);
            Sharpening.Radius = Convert.ToDouble(NewVals[12]);
            Sharpening.Amount = Convert.ToInt32(NewVals[13]);
            Sharpening.EdgedetectionRadius = Convert.ToDouble(NewVals[14]);
            Sharpening.EdgeTolerance = Convert.ToInt32(NewVals[15]);
            Sharpening.HalocontrolAmount = Convert.ToInt32(NewVals[16]);
            Sharpening.DeconvRadius = Convert.ToDouble(NewVals[17]);
            Sharpening.DeconvAmount = Convert.ToInt32(NewVals[18]);
            Sharpening.DeconvDamping = Convert.ToInt32(NewVals[19]);
            Sharpening.DeconvIterations = Convert.ToInt32(NewVals[20]);
            Vibrance.Pastels = Convert.ToInt32(NewVals[21]);
            Vibrance.Saturated = Convert.ToInt32(NewVals[22]);
            SharpenEdge.Passes = Convert.ToInt32(NewVals[23]);
            SharpenEdge.Strength = Convert.ToDouble(NewVals[24]);
            SharpenMicro.Strength = Convert.ToDouble(NewVals[25]);
            SharpenMicro.Uniformity = Convert.ToDouble(NewVals[26]);
            WhiteBalance.Temperature = Convert.ToInt32(NewVals[27]);
            WhiteBalance.Green = Convert.ToDouble(NewVals[28]);
            ImpulseDenoising.Threshold = Convert.ToInt32(NewVals[29]);
            Defringing.Radius = Convert.ToDouble(NewVals[30]);
            Defringing.Threshold = Convert.ToInt32(NewVals[31]);
            DirectionalPyramidDenoising.Luma = Convert.ToInt32(NewVals[32]);
            DirectionalPyramidDenoising.Chroma = Convert.ToInt32(NewVals[33]);
            DirectionalPyramidDenoising.Gamma = Convert.ToDouble(NewVals[34]);
            EPD.Strength = Convert.ToDouble(NewVals[35]);
            EPD.EdgeStopping = Convert.ToDouble(NewVals[36]);
            EPD.Scale = Convert.ToDouble(NewVals[37]);
            EPD.ReweightingIterates = Convert.ToInt32(NewVals[38]);
            ShadowsAndHighlights.Highlights = Convert.ToInt32(NewVals[39]);
            ShadowsAndHighlights.HighlightTonalWidth = Convert.ToInt32(NewVals[40]);
            ShadowsAndHighlights.Shadows = Convert.ToInt32(NewVals[41]);
            ShadowsAndHighlights.ShadowTonalWidth = Convert.ToInt32(NewVals[42]);
            ShadowsAndHighlights.LocalContrast = Convert.ToInt32(NewVals[43]);
            ShadowsAndHighlights.Radius = Convert.ToInt32(NewVals[44]);
            Crop.X = Convert.ToInt32(NewVals[45]);
            Crop.Y = Convert.ToInt32(NewVals[46]);
            Rotation.Degree = Convert.ToDouble(NewVals[47]);
            Distortion.Amount = Convert.ToDouble(NewVals[48]);
            Perspective.Horizontal = Convert.ToInt32(NewVals[49]);
            Perspective.Vertical = Convert.ToInt32(NewVals[50]);
            CACorrection.Red = Convert.ToDouble(NewVals[51]);
            CACorrection.Blue = Convert.ToDouble(NewVals[52]);
            VignettingCorrection.Amount = Convert.ToInt32(NewVals[53]);
            VignettingCorrection.Radius = Convert.ToInt32(NewVals[54]);
            VignettingCorrection.Strength = Convert.ToInt32(NewVals[55]);
            VignettingCorrection.CenterX = Convert.ToInt32(NewVals[56]);
            VignettingCorrection.CenterY = Convert.ToInt32(NewVals[57]);
            Resize.Scale = Convert.ToDouble(NewVals[58]);
            Resize.Width = Convert.ToInt32(NewVals[59]);
            Resize.Height = Convert.ToInt32(NewVals[60]);
            ColorManagement.GammaValue = Convert.ToDouble(NewVals[61]);
            ColorManagement.GammaSlope = Convert.ToDouble(NewVals[62]);
            DirectionalPyramidEqualizer.Mult0 = Convert.ToDouble(NewVals[63]);
            DirectionalPyramidEqualizer.Mult1 = Convert.ToDouble(NewVals[64]);
            DirectionalPyramidEqualizer.Mult2 = Convert.ToDouble(NewVals[65]);
            DirectionalPyramidEqualizer.Mult3 = Convert.ToDouble(NewVals[66]);
            DirectionalPyramidEqualizer.Mult4 = Convert.ToDouble(NewVals[67]);
            RAW.FlatFieldBlurRadius = Convert.ToInt32(NewVals[68]);
            RAW.CARed = Convert.ToDouble(NewVals[69]);
            RAW.CABlue = Convert.ToDouble(NewVals[70]);
            RAW.LineDenoise = Convert.ToInt32(NewVals[71]);
            RAW.GreenEqThreshold = Convert.ToInt32(NewVals[72]);
            RAW.CcSteps = Convert.ToInt32(NewVals[73]);
            RAW.DCBIterations = Convert.ToInt32(NewVals[74]);
            RAW.PreExposure = Convert.ToDouble(NewVals[75]);
            RAW.PrePreserv = Convert.ToDouble(NewVals[76]);
            RAW.PreBlackzero = Convert.ToDouble(NewVals[77]);
            RAW.PreBlackone = Convert.ToDouble(NewVals[78]);
            RAW.PreBlacktwo = Convert.ToDouble(NewVals[79]);
            RAW.PreBlackthree = Convert.ToDouble(NewVals[80]);
        }

        #endregion
    }
}