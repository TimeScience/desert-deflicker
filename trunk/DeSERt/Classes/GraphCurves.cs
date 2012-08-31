using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DeSERt
{
    [Serializable()]
    public class GCurves
    {
        #region Variables and Enumerator

        public Dictionary<string, GCurve> Curves { get { return crvs; } }
        public GCurve BrInCurve { get; set; }
        private Dictionary<string, GCurve> crvs;

        public enum CurveName
        {
            BrightnessInput = -1,
            Exposure_Compensation = 0,

            Exposure_Clip,
            Exposure_Brightness,
            Exposure_Contrast,
            Exposure_Saturation,
            Exposure_Black,
            Exposure_HighlightCompr,
            Exposure_HighlightComprThreshold,
            Exposure_ShadowCompr,
            LuminanceCurve_Brightness,
            LuminanceCurve_Contrast,
            LuminanceCurve_Saturation,
            LuminanceCurve_SaturationLimit,
            Sharpening_Radius,
            Sharpening_Amount,
            Sharpening_Threshold,
            Sharpening_EdgedetectionRadius,
            Sharpening_EdgeTolerance,
            Sharpening_HalocontrolAmount,
            Sharpening_DeconvRadius,
            Sharpening_DeconvAmount,
            Sharpening_DeconvDamping,
            Sharpening_DeconvIterations,
            Vibrance_Pastels,
            Vibrance_Saturated,
            Vibrance_PSThreshold,
            SharpenEdge_Passes,
            SharpenEdge_Strength,
            SharpenMicro_Strength,
            SharpenMicro_Uniformity,
            WhiteBalance_Temperature,
            WhiteBalance_Green,
            ImpulseDenoising_Threshold,
            Defringing_Radius,
            Defringing_Threshold,
            DirectionalPyramidDenoising_Luma,
            DirectionalPyramidDenoising_Chroma,
            DirectionalPyramidDenoising_Gamma,
            EPD_Strength,
            EPD_EdgeStopping,
            EPD_Scale,
            EPD_ReweightingIterates,
            ShadowsAndHighlights_Highlights,
            ShadowsAndHighlights_HighlightTonalWidth,
            ShadowsAndHighlights_Shadows,
            ShadowsAndHighlights_ShadowTonalWidth,
            ShadowsAndHighlights_LocalContrast,
            ShadowsAndHighlights_Radius,
            Crop_X,
            Crop_Y,
            Rotation_Degree,
            Distortion_Amount,
            Perspective_Horizontal,
            Perspective_Vertical,
            CACorrection_Red,
            CACorrection_Blue,
            VignettingCorrection_Amount,
            VignettingCorrection_Radius,
            VignettingCorrection_Strength,
            VignettingCorrection_CenterX,
            VignettingCorrection_CenterY,
            Resize_Scale,
            Resize_Width,
            Resize_Height,
            ColorManagement_GammaValue,
            ColorManagement_GammaSlope,
            DirectionalPyramidEqualizer_Mult0,
            DirectionalPyramidEqualizer_Mult1,
            DirectionalPyramidEqualizer_Mult2,
            DirectionalPyramidEqualizer_Mult3,
            DirectionalPyramidEqualizer_Mult4,
            RAW_FlatFieldBlurRadius,
            RAW_CARed,
            RAW_CABlue,
            RAW_LineDenoise,
            RAW_GreenEqThreshold,
            RAW_CcSteps,
            RAW_DCBIterations,
            RAW_PreExposure,
            RAW_PrePreserv,
            RAW_PreBlackzero,
            RAW_PreBlackone,
            RAW_PreBlacktwo,
            RAW_PreBlackthree,
        }

        #endregion

        #region Constructer

        public GCurves()
        {
            crvs = new Dictionary<string, GCurve>();
        }

        #endregion

        #region Subroutines

        public void InitBrCurves(List<double> Brightness)
        {
            BrInCurve = new GCurve(CurveName.BrightnessInput);
            for (int i = 0; i < Brightness.Count; i++)
            {
                BrInCurve.Points.Add(new KeyValuePair<bool,PointF>(false, new PointF(i, (float)Brightness[i])));
            }

            double min = Brightness.Min() - (Brightness.Min() * 5 / 100);
            double max = Brightness.Max() + (Brightness.Min() * 5 / 100);
            crvs.Add(CurveName.Exposure_Compensation.ToString(), new GCurve(CurveName.Exposure_Compensation, min, max, (float)Brightness[0], (float)Brightness[Brightness.Count - 1], Brightness.Count));
        }

        public void UpdateBrCurve(List<double> Brightness, double change, int startIndex)
        {
            BrInCurve.ClearPoints();
            for (int i = 0; i < Brightness.Count; i++)
            {
                BrInCurve.Points.Add(new KeyValuePair<bool, PointF>(false, new PointF(i, (float)Brightness[i])));
            }

            double min = Brightness.Min() - (Brightness.Min() * 5 / 100);
            double max = Brightness.Max() + (Brightness.Min() * 5 / 100);
            string n = CurveName.Exposure_Compensation.ToString();
            crvs[n].SetMinMax(min, max);
            for (int i = 0; i < crvs[n].Points.Count; i++)
            {
                if (crvs[n].Points[i].Value.X >= startIndex)
                {
                    crvs[n].Points[i] = new KeyValuePair<bool, PointF>(crvs[n].Points[i].Key, new PointF(crvs[n].Points[i].Value.X, (float)(crvs[n].Points[i].Value.Y + change)));
                }
            }
        }

        public void InitPP3Curves(PP3Values PP3, int filecount)
        {
            crvs.Add(CurveName.Exposure_Clip.ToString(), new GCurve(CurveName.Exposure_Clip, 0, 0.99990000000000001, (float)PP3.Exposure.Clip, filecount));
            crvs.Add(CurveName.Exposure_Brightness.ToString(), new GCurve(CurveName.Exposure_Brightness, -100, 100, PP3.Exposure.Brightness, filecount));
            crvs.Add(CurveName.Exposure_Contrast.ToString(), new GCurve(CurveName.Exposure_Contrast, -100, 100, PP3.Exposure.Contrast, filecount));
            crvs.Add(CurveName.Exposure_Saturation.ToString(), new GCurve(CurveName.Exposure_Saturation, -100, 100, PP3.Exposure.Saturation, filecount));
            crvs.Add(CurveName.Exposure_Black.ToString(), new GCurve(CurveName.Exposure_Black, -16384, 32768, PP3.Exposure.Black, filecount));
            crvs.Add(CurveName.Exposure_HighlightCompr.ToString(), new GCurve(CurveName.Exposure_HighlightCompr, 0, 500, PP3.Exposure.HighlightCompr, filecount));
            crvs.Add(CurveName.Exposure_HighlightComprThreshold.ToString(), new GCurve(CurveName.Exposure_HighlightComprThreshold, 0, 100, PP3.Exposure.HighlightComprThreshold, filecount));
            crvs.Add(CurveName.Exposure_ShadowCompr.ToString(), new GCurve(CurveName.Exposure_ShadowCompr, 0, 100, PP3.Exposure.ShadowCompr, filecount));
            crvs.Add(CurveName.LuminanceCurve_Brightness.ToString(), new GCurve(CurveName.LuminanceCurve_Brightness, -100, 100, PP3.LuminanceCurve.Brightness, filecount));
            crvs.Add(CurveName.LuminanceCurve_Contrast.ToString(), new GCurve(CurveName.LuminanceCurve_Contrast, -100, 100, PP3.LuminanceCurve.Contrast, filecount));
            crvs.Add(CurveName.LuminanceCurve_Saturation.ToString(), new GCurve(CurveName.LuminanceCurve_Saturation, -100, 100, PP3.LuminanceCurve.Saturation, filecount));
            crvs.Add(CurveName.LuminanceCurve_SaturationLimit.ToString(), new GCurve(CurveName.LuminanceCurve_SaturationLimit, 0, 100, PP3.LuminanceCurve.SaturationLimit, filecount));
            crvs.Add(CurveName.Sharpening_Radius.ToString(), new GCurve(CurveName.Sharpening_Radius, 0.29999999999999999, 3, (float)PP3.Sharpening.Radius, filecount));
            crvs.Add(CurveName.Sharpening_Amount.ToString(), new GCurve(CurveName.Sharpening_Amount, 1, 1000, PP3.Sharpening.Amount, filecount));
            crvs.Add(CurveName.Sharpening_Threshold.ToString(), new GCurve(CurveName.Sharpening_Threshold, 0, 16384, PP3.Sharpening.Threshold, filecount));
            crvs.Add(CurveName.Sharpening_EdgedetectionRadius.ToString(), new GCurve(CurveName.Sharpening_EdgedetectionRadius, 1.8999999999999999, 1.8999999999999999, (float)PP3.Sharpening.EdgedetectionRadius, filecount));
            crvs.Add(CurveName.Sharpening_EdgeTolerance.ToString(), new GCurve(CurveName.Sharpening_EdgeTolerance, 1800, 1800, PP3.Sharpening.EdgeTolerance, filecount));
            crvs.Add(CurveName.Sharpening_HalocontrolAmount.ToString(), new GCurve(CurveName.Sharpening_HalocontrolAmount, 85, 85, PP3.Sharpening.HalocontrolAmount, filecount));
            crvs.Add(CurveName.Sharpening_DeconvRadius.ToString(), new GCurve(CurveName.Sharpening_DeconvRadius, 0.5, 0.75, (float)PP3.Sharpening.DeconvRadius, filecount));
            crvs.Add(CurveName.Sharpening_DeconvAmount.ToString(), new GCurve(CurveName.Sharpening_DeconvAmount, 0, 75, PP3.Sharpening.DeconvAmount, filecount));
            crvs.Add(CurveName.Sharpening_DeconvDamping.ToString(), new GCurve(CurveName.Sharpening_DeconvDamping, 0, 20, PP3.Sharpening.DeconvDamping, filecount));
            crvs.Add(CurveName.Sharpening_DeconvIterations.ToString(), new GCurve(CurveName.Sharpening_DeconvIterations, 5, 30, PP3.Sharpening.DeconvIterations, filecount));
            crvs.Add(CurveName.Vibrance_Pastels.ToString(), new GCurve(CurveName.Vibrance_Pastels, -100, 100, PP3.Vibrance.Pastels, filecount));
            crvs.Add(CurveName.Vibrance_Saturated.ToString(), new GCurve(CurveName.Vibrance_Saturated, -100, 100, PP3.Vibrance.Saturated, filecount));
            crvs.Add(CurveName.Vibrance_PSThreshold.ToString(), new GCurve(CurveName.Vibrance_PSThreshold, 0, 100, PP3.Vibrance.PSThreshold, filecount));
            crvs.Add(CurveName.SharpenEdge_Passes.ToString(), new GCurve(CurveName.SharpenEdge_Passes, 1, 4, PP3.SharpenEdge.Passes, filecount));
            crvs.Add(CurveName.SharpenEdge_Strength.ToString(), new GCurve(CurveName.SharpenEdge_Strength, 0, 100, (float)PP3.SharpenEdge.Strength, filecount));
            crvs.Add(CurveName.SharpenMicro_Strength.ToString(), new GCurve(CurveName.SharpenMicro_Strength, 0, 100, (float)PP3.SharpenMicro.Strength, filecount));
            crvs.Add(CurveName.SharpenMicro_Uniformity.ToString(), new GCurve(CurveName.SharpenMicro_Uniformity, 0, 100, (float)PP3.SharpenMicro.Uniformity, filecount));
            crvs.Add(CurveName.WhiteBalance_Temperature.ToString(), new GCurve(CurveName.WhiteBalance_Temperature, 2000, 25000, PP3.WhiteBalance.Temperature, filecount));
            crvs.Add(CurveName.WhiteBalance_Green.ToString(), new GCurve(CurveName.WhiteBalance_Green, 0.02, 5, (float)PP3.WhiteBalance.Green, filecount));
            crvs.Add(CurveName.ImpulseDenoising_Threshold.ToString(), new GCurve(CurveName.ImpulseDenoising_Threshold, 0, 100, PP3.ImpulseDenoising.Threshold, filecount));
            crvs.Add(CurveName.Defringing_Radius.ToString(), new GCurve(CurveName.Defringing_Radius, 0.5, 5, (float)PP3.Defringing.Radius, filecount));
            crvs.Add(CurveName.Defringing_Threshold.ToString(), new GCurve(CurveName.Defringing_Threshold, 0, 100, PP3.Defringing.Threshold, filecount));
            crvs.Add(CurveName.DirectionalPyramidDenoising_Luma.ToString(), new GCurve(CurveName.DirectionalPyramidDenoising_Luma, 0, 100, PP3.DirectionalPyramidDenoising.Luma, filecount));
            crvs.Add(CurveName.DirectionalPyramidDenoising_Chroma.ToString(), new GCurve(CurveName.DirectionalPyramidDenoising_Chroma, 0, 100, PP3.DirectionalPyramidDenoising.Chroma, filecount));
            crvs.Add(CurveName.DirectionalPyramidDenoising_Gamma.ToString(), new GCurve(CurveName.DirectionalPyramidDenoising_Gamma, 1, 3, (float)PP3.DirectionalPyramidDenoising.Gamma, filecount));
            crvs.Add(CurveName.EPD_Strength.ToString(), new GCurve(CurveName.EPD_Strength, -2, 0.25, (float)PP3.EPD.Strength, filecount));
            crvs.Add(CurveName.EPD_EdgeStopping.ToString(), new GCurve(CurveName.EPD_EdgeStopping, 0.10000000000000001, 1.3999999999999999, (float)PP3.EPD.EdgeStopping, filecount));
            crvs.Add(CurveName.EPD_Scale.ToString(), new GCurve(CurveName.EPD_Scale, 0.10000000000000001, 1, (float)PP3.EPD.Scale, filecount));
            crvs.Add(CurveName.EPD_ReweightingIterates.ToString(), new GCurve(CurveName.EPD_ReweightingIterates, 0, 0, PP3.EPD.ReweightingIterates, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_Highlights.ToString(), new GCurve(CurveName.ShadowsAndHighlights_Highlights, 0, 100, PP3.ShadowsAndHighlights.Highlights, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_HighlightTonalWidth.ToString(), new GCurve(CurveName.ShadowsAndHighlights_HighlightTonalWidth, 10, 100, PP3.ShadowsAndHighlights.HighlightTonalWidth, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_Shadows.ToString(), new GCurve(CurveName.ShadowsAndHighlights_Shadows, 0, 100, PP3.ShadowsAndHighlights.Shadows, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_ShadowTonalWidth.ToString(), new GCurve(CurveName.ShadowsAndHighlights_ShadowTonalWidth, 10, 100, PP3.ShadowsAndHighlights.ShadowTonalWidth, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_LocalContrast.ToString(), new GCurve(CurveName.ShadowsAndHighlights_LocalContrast, 0, 100, PP3.ShadowsAndHighlights.LocalContrast, filecount));
            crvs.Add(CurveName.ShadowsAndHighlights_Radius.ToString(), new GCurve(CurveName.ShadowsAndHighlights_Radius, 5, 100, PP3.ShadowsAndHighlights.Radius, filecount));
            crvs.Add(CurveName.Crop_X.ToString(), new GCurve(CurveName.Crop_X, PP3.Crop.X, filecount));
            crvs.Add(CurveName.Crop_Y.ToString(), new GCurve(CurveName.Crop_Y, PP3.Crop.Y, filecount));
            crvs.Add(CurveName.Rotation_Degree.ToString(), new GCurve(CurveName.Rotation_Degree, -45, 45, (float)PP3.Rotation.Degree, filecount));
            crvs.Add(CurveName.Distortion_Amount.ToString(), new GCurve(CurveName.Distortion_Amount, -0.5, 0.5, (float)PP3.Distortion.Amount, filecount));
            crvs.Add(CurveName.Perspective_Horizontal.ToString(), new GCurve(CurveName.Perspective_Horizontal, -100, 100, PP3.Perspective.Horizontal, filecount));
            crvs.Add(CurveName.Perspective_Vertical.ToString(), new GCurve(CurveName.Perspective_Vertical, -100, 100, PP3.Perspective.Vertical, filecount));
            crvs.Add(CurveName.CACorrection_Red.ToString(), new GCurve(CurveName.CACorrection_Red, -0.0050000000000000001, 0.0050000000000000001, (float)PP3.CACorrection.Red, filecount));
            crvs.Add(CurveName.CACorrection_Blue.ToString(), new GCurve(CurveName.CACorrection_Blue, -0.0050000000000000001, 0.0050000000000000001, (float)PP3.CACorrection.Blue, filecount));
            crvs.Add(CurveName.VignettingCorrection_Amount.ToString(), new GCurve(CurveName.VignettingCorrection_Amount, -100, 100, PP3.VignettingCorrection.Amount, filecount));
            crvs.Add(CurveName.VignettingCorrection_Radius.ToString(), new GCurve(CurveName.VignettingCorrection_Radius, 0, 100, PP3.VignettingCorrection.Radius, filecount));
            crvs.Add(CurveName.VignettingCorrection_Strength.ToString(), new GCurve(CurveName.VignettingCorrection_Strength, 1, 100, PP3.VignettingCorrection.Strength, filecount));
            crvs.Add(CurveName.VignettingCorrection_CenterX.ToString(), new GCurve(CurveName.VignettingCorrection_CenterX, -100, 100, PP3.VignettingCorrection.CenterX, filecount));
            crvs.Add(CurveName.VignettingCorrection_CenterY.ToString(), new GCurve(CurveName.VignettingCorrection_CenterY, -100, 100, PP3.VignettingCorrection.CenterY, filecount));
            crvs.Add(CurveName.Resize_Scale.ToString(), new GCurve(CurveName.Resize_Scale, 0.01, 4, (float)PP3.Resize.Scale, filecount));
            crvs.Add(CurveName.Resize_Width.ToString(), new GCurve(CurveName.Resize_Width, 32, 15600, PP3.Resize.Width, filecount));
            crvs.Add(CurveName.Resize_Height.ToString(), new GCurve(CurveName.Resize_Height, 32, 10376, PP3.Resize.Height, filecount));
            crvs.Add(CurveName.ColorManagement_GammaValue.ToString(), new GCurve(CurveName.ColorManagement_GammaValue, 1, 3.5, (float)PP3.ColorManagement.GammaValue, filecount));
            crvs.Add(CurveName.ColorManagement_GammaSlope.ToString(), new GCurve(CurveName.ColorManagement_GammaSlope, 0, 15, (float)PP3.ColorManagement.GammaSlope, filecount));
            crvs.Add(CurveName.DirectionalPyramidEqualizer_Mult0.ToString(), new GCurve(CurveName.DirectionalPyramidEqualizer_Mult0, 0, 4, (float)PP3.DirectionalPyramidEqualizer.Mult0, filecount));
            crvs.Add(CurveName.DirectionalPyramidEqualizer_Mult1.ToString(), new GCurve(CurveName.DirectionalPyramidEqualizer_Mult1, 0, 4, (float)PP3.DirectionalPyramidEqualizer.Mult1, filecount));
            crvs.Add(CurveName.DirectionalPyramidEqualizer_Mult2.ToString(), new GCurve(CurveName.DirectionalPyramidEqualizer_Mult2, 0, 4, (float)PP3.DirectionalPyramidEqualizer.Mult2, filecount));
            crvs.Add(CurveName.DirectionalPyramidEqualizer_Mult3.ToString(), new GCurve(CurveName.DirectionalPyramidEqualizer_Mult3, 0, 4, (float)PP3.DirectionalPyramidEqualizer.Mult3, filecount));
            crvs.Add(CurveName.DirectionalPyramidEqualizer_Mult4.ToString(), new GCurve(CurveName.DirectionalPyramidEqualizer_Mult4, 0, 1, (float)PP3.DirectionalPyramidEqualizer.Mult4, filecount));
            crvs.Add(CurveName.RAW_FlatFieldBlurRadius.ToString(), new GCurve(CurveName.RAW_FlatFieldBlurRadius, 0, 200, PP3.RAW.FlatFieldBlurRadius, filecount));
            crvs.Add(CurveName.RAW_CARed.ToString(), new GCurve(CurveName.RAW_CARed, -4, 4, (float)PP3.RAW.CARed, filecount));
            crvs.Add(CurveName.RAW_CABlue.ToString(), new GCurve(CurveName.RAW_CABlue, -4, 4, (float)PP3.RAW.CABlue, filecount));
            crvs.Add(CurveName.RAW_LineDenoise.ToString(), new GCurve(CurveName.RAW_LineDenoise, 0, 1000, PP3.RAW.LineDenoise, filecount));
            crvs.Add(CurveName.RAW_GreenEqThreshold.ToString(), new GCurve(CurveName.RAW_GreenEqThreshold, 0, 100, PP3.RAW.GreenEqThreshold, filecount));
            crvs.Add(CurveName.RAW_CcSteps.ToString(), new GCurve(CurveName.RAW_CcSteps, 0, 5, PP3.RAW.CcSteps, filecount));
            crvs.Add(CurveName.RAW_DCBIterations.ToString(), new GCurve(CurveName.RAW_DCBIterations, 2, 2, PP3.RAW.DCBIterations, filecount));
            crvs.Add(CurveName.RAW_PreExposure.ToString(), new GCurve(CurveName.RAW_PreExposure, 0.10000000000000001, 16, (float)PP3.RAW.PreExposure, filecount));
            crvs.Add(CurveName.RAW_PrePreserv.ToString(), new GCurve(CurveName.RAW_PrePreserv, 0, 2.5, (float)PP3.RAW.PrePreserv, filecount));
            crvs.Add(CurveName.RAW_PreBlackzero.ToString(), new GCurve(CurveName.RAW_PreBlackzero, -50, 50, (float)PP3.RAW.PreBlackzero, filecount));
            crvs.Add(CurveName.RAW_PreBlackone.ToString(), new GCurve(CurveName.RAW_PreBlackone, -50, 50, (float)PP3.RAW.PreBlackone, filecount));
            crvs.Add(CurveName.RAW_PreBlacktwo.ToString(), new GCurve(CurveName.RAW_PreBlacktwo, -50, 50, (float)PP3.RAW.PreBlacktwo, filecount));
            crvs.Add(CurveName.RAW_PreBlackthree.ToString(), new GCurve(CurveName.RAW_PreBlackthree, -50, 50, (float)PP3.RAW.PreBlackthree, filecount));
        }

        public void ResetPP3Curves()
        {
            for (int i = 0; i < Curves.Count; i++)
            {
                if (Curves.ElementAt(i).Value.Name != CurveName.Exposure_Compensation.ToString())
                {
                    crvs.Remove(Curves.ElementAt(i).Key);
                    i--;
                }
            }
        }

        //not written yet
        public void InitXMPCurves()
        {

        }

        public GCurve GetCurve(CurveName Name)
        {
            GCurve output;
            Curves.TryGetValue(Name.ToString(), out output);
            return output;
        }

        public string[] GetAllCurvenames()
        {
            string[] output = new string[Curves.Count];
            for (int i = 0; i < Curves.Count; i++)
            {
                output[i] = Curves.Keys.ElementAt(i);
            }
            return output;
        }

        public void Clear()
        {
            crvs.Clear();
            BrInCurve = null;
        }

        #endregion

        #region Subclass

        [Serializable()]
        public class GCurve
        {
            public List<KeyValuePair<bool, PointF>> Points { get; set; }
            public string Name { get { return name; } }
            public double min { get { return minval; } }
            public double max { get { return maxval; } }
            public int SelectedPoint { get; set; }

            private string name;
            private double minval;
            private double maxval;
            private float init0;
            private float init1;

            #region Constructor

            public GCurve(CurveName Curvename, double min, double max, float firstVal, float lastVal, int filecount)
            {
                name = Curvename.ToString();
                minval = min;
                maxval = max;
                init0 = firstVal;
                init1 = lastVal;
                SelectedPoint = 0;
                Points = new List<KeyValuePair<bool, PointF>>();
                ResetCurve(filecount);
            }

            public GCurve(CurveName Curvename, double min, double max, float Val, int filecount)
            {
                name = Curvename.ToString();
                minval = min;
                maxval = max;
                init0 = Val;
                init1 = Val;
                SelectedPoint = 0;
                Points = new List<KeyValuePair<bool, PointF>>();
                ResetCurve(filecount);
            }

            public GCurve(CurveName Curvename, float Val, int filecount)
            {
                name = Curvename.ToString();
                minval = double.MinValue;
                maxval = double.MaxValue;
                init0 = Val;
                init1 = Val;
                SelectedPoint = 0;
                Points = new List<KeyValuePair<bool, PointF>>();
                ResetCurve(filecount);
            }

            public GCurve(CurveName Curvename)
            {
                name = Curvename.ToString();
                minval = double.MinValue;
                maxval = double.MaxValue;
                init0 = 0;
                init1 = 0;
                Points = new List<KeyValuePair<bool, PointF>>();
            }

            #endregion

            #region Subroutines

            #region Add Point

            public void AddPoint(PointF inPoint)
            {
                AddPoint(inPoint, false);
            }

            public void AddPoint(float x, float y)
            {
                AddPoint(new PointF(x, y), false);
            }

            public void AddPoint(float x, float y, bool iskeyframe)
            {
                AddPoint(new PointF(x, y), iskeyframe);
            }

            public void AddPoint(PointF inPoint, bool iskeyframe)
            {
                if (Points.Count < 2)
                {
                    Points.Add(new KeyValuePair<bool, PointF>(iskeyframe, inPoint));
                }
                else
                {
                    KeyValuePair<bool, PointF>[] tmpList = new KeyValuePair<bool, PointF>[Points.Count];
                    Points.CopyTo(tmpList);
                    Points.Clear();

                    for (int i = 1; i < tmpList.Length; i++)
                    {
                        //check if first point and new point are the same
                        if (i == 1 && tmpList[0].Value.X != inPoint.X)
                        {
                            Points.Add(tmpList[0]);
                        }
                        else if (i == 1 && tmpList[0].Value.X == inPoint.X)
                        {
                            Points.Add(new KeyValuePair<bool, PointF>(true, inPoint));
                        }
                        
                        //add new point if not hitting some other point
                        if (tmpList[i - 1].Value.X < inPoint.X && tmpList[i].Value.X > inPoint.X)
                        {
                            Points.Add(new KeyValuePair<bool, PointF>(iskeyframe, inPoint));
                            if (iskeyframe) { SelectedPoint = i - 1; }
                            else { SelectedPoint = i; }
                        }

                        //heck if current point and new point are the same
                        if (tmpList[i].Value.X == inPoint.X)
                        {
                            Points.Add(new KeyValuePair<bool, PointF>(true, inPoint));
                            SelectedPoint = i - 1;
                        }
                        else { Points.Add(tmpList[i]); }
                    }
                }
            }

            #endregion

            public void SetMinMax(double NewMin, double NewMax)
            {
                minval = NewMin;
                maxval = NewMax;
            }

            public void RemovePoint(int index)
            {
                Points.RemoveAt(index);
                if (SelectedPoint == index) { SelectedPoint--; }
            }

            public void ClearPoints()
            {
                if (this != null) { if (Points != null) { Points.Clear(); } }
            }

            public void ResetCurve(int filecount)
            {
                Points.Clear();
                if (name == CurveName.Exposure_Compensation.ToString())
                {
                    Points.Add(new KeyValuePair<bool, PointF>(false, new PointF(0, init0)));
                    Points.Add(new KeyValuePair<bool, PointF>(false, new PointF(filecount - 1, init1)));
                }
                else
                {
                    Points.Add(new KeyValuePair<bool, PointF>(true, new PointF(0, init0)));
                    Points.Add(new KeyValuePair<bool, PointF>(false, new PointF(filecount - 1, init1)));
                }
            }
            
            public PointF[] GetSmoothCurve(int outputCount)
            {
                PointF[] OutPoints = new PointF[outputCount];
                PointF[] TmpCalcPoints;
                PointF[] InPoints = new PointF[Points.Count];
                for (int i = 0; i < Points.Count; i++) { InPoints[i] = Points[i].Value; }

                if (Points.Min(p => p.Value.X) == Points.Max(p => p.Value.X) && Points.Min(p => p.Value.Y) == Points.Max(p => p.Value.Y))
                {
                    for (int i = 0; i < outputCount; i++) { OutPoints[i] = (Points[0].Value); }
                }
                else if (Points.Count != outputCount)
                {
                    int factor;
                    if (outputCount > 60 || Points.Count == 2) { factor = 1; }
                    else if (outputCount > 20) { factor = 3; }
                    else if (outputCount > 10) { factor = 5; }
                    else { factor = 10; }

                    MySpline.CalcSpline(InPoints, outputCount * factor, out TmpCalcPoints, (float)this.min, (float)this.max);

                    factor = Convert.ToInt32(Math.Floor((double)TmpCalcPoints.Length / ((double)outputCount - 1)));

                    for (int i = 0; i < outputCount; i++) { OutPoints[i] = TmpCalcPoints[i * factor]; }
                }
                else { for (int i = 0; i < outputCount; i++) { OutPoints[i] = Points[i].Value; } }

                OutPoints[outputCount - 1] = InPoints[InPoints.Length - 1];

                return OutPoints;
            }

            #endregion
        }

        #endregion
    }
}
