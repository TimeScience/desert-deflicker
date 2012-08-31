using System;
using System.Collections.Generic;
using System.Drawing;

namespace DeSERt
{
    internal struct MySpline
    {
        //The basis of this code comes from:
        //http://www.codeproject.com/Articles/12018/Interpolation-with-Polynomials-and-Splines

        private static void solveTridiag(float[] sub, float[] diag, float[] sup, ref float[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }
            b[n] = b[n] / diag[n];
            for (i = n - 1; i >= 1; i--)
            {
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
            }
        }

        public static void CalcSpline(PointF[] points, int filecount, out float[] output, float minVal, float maxVal)
        {
            PointF[] tmpoutput;
            CalcSpline(points, filecount, out tmpoutput, minVal, maxVal);
            output = new float[tmpoutput.Length];

            for (int i = 0; i < tmpoutput.Length; i++)
            {
                output[i] = tmpoutput[i].Y;
            }
        }

        public static void CalcSpline(PointF[] points, int filecount, out float[] output)
        {
            output = new float[1];
            CalcSpline(points, filecount, out output, float.MinValue, float.MaxValue);
        }

        public static void CalcSpline(PointF[] points, int filecount, out PointF[] output)
        {
            output = new PointF[1];
            CalcSpline(points, filecount, out output, float.MinValue, float.MaxValue);
        }

        public static void CalcSpline(List<PointF> points, int filecount, out float[] output)
        {
            output = new float[1];
            PointF[] dPoints = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                dPoints[i] = points[i];
            }
            CalcSpline(dPoints, filecount, out output, float.MinValue, float.MaxValue);
        }

        public static void CalcSpline(List<PointF> points, int filecount, out float[] output, float minVal, float maxVal)
        {
            PointF[] tmpoutput;
            PointF[] dPoints = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                dPoints[i] = points[i];
            }
            CalcSpline(dPoints, filecount, out tmpoutput, minVal, maxVal);
            output = new float[tmpoutput.Length];

            for (int i = 0; i < tmpoutput.Length; i++)
            {
                output[i] = tmpoutput[i].Y;
            }
        }

        public static void CalcSpline(List<PointF> points, int filecount, out PointF[] output)
        {
            output = new PointF[1];
            PointF[] dPoints = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                dPoints[i] = points[i];
            }
            CalcSpline(dPoints, filecount, out output, float.MinValue, float.MaxValue);
        }

        public static void CalcSpline(List<PointF> points, int filecount, out PointF[] output, float minVal, float maxVal)
        {
            output = new PointF[1];
            PointF[] dPoints = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                dPoints[i] = points[i];
            }
            CalcSpline(dPoints, filecount, out output, minVal, maxVal);
        }

        public static void CalcSpline(PointF[] points, int filecount, out PointF[] output, float minVal, float maxVal)
        {
            int Pcount = points.Length;
            output = new PointF[filecount];

            //to find out where the curve calculation should start: (in case there are equal Y values wich should build a straight line)
            int start = 0;
            for (int i = 1; i < Pcount; i++)
            {
                if (points[i].Y != points[i - 1].Y)
                {
                    start = i - 1;
                    break;
                }
            }

            if (Pcount == 2)
            {
                float stepY = (points[points.Length - 1].Y - points[0].Y) / (float)(filecount - 1);
                float stepX = (points[points.Length - 1].X - points[0].X) / (float)(filecount - 1);
                for (int i = 0; i < filecount; i++)
                {
                    output[i].X = points[0].X + (stepX * i);
                    output[i].Y = points[0].Y + (stepY * i);
                    if (output[i].Y < minVal) { output[i].Y = minVal; }
                    else if (output[i].Y > maxVal) { output[i].Y = maxVal; }
                }
            }
            else if (Pcount > 2)
            {
                float[] yCoords = new float[Pcount];        // Newton form coefficients
                float[] xCoords = new float[Pcount];        // x-coordinates of nodes
                float y;
                float x;

                int precision = (filecount - 1) / (Pcount - 1);
                int npp = (Pcount * precision);                     // number of points used for drawing

                for (int i = 0; i < Pcount; i++)
                {
                    xCoords[i] = points[i].X;
                    yCoords[i] = points[i].Y;
                }

                float[] a = new float[Pcount];
                float x1;
                float x2;
                float[] h = new float[Pcount];
                for (int i = 1; i < Pcount; i++)
                {
                    h[i] = xCoords[i] - xCoords[i - 1];
                }

                float[] sub = new float[Pcount - 1];
                float[] diag = new float[Pcount - 1];
                float[] sup = new float[Pcount - 1];

                for (int i = 1; i < Pcount - 1; i++)
                {
                    diag[i] = (h[i] + h[i + 1]) / 3;
                    sup[i] = h[i + 1] / 6;
                    sub[i] = h[i] / 6;
                    a[i] = (yCoords[i + 1] - yCoords[i]) / h[i + 1] - (yCoords[i] - yCoords[i - 1]) / h[i];
                }
                solveTridiag(sub, diag, sup, ref a, Pcount - 2);

                int count = 1;

                output[0].X = points[0].X;
                output[0].Y = points[0].Y;

                for (int i = 1; i < Pcount; i++)
                {
                    for (int j = 1; j <= precision; j++)
                    {
                        x1 = (h[i] * j) / precision;
                        x2 = h[i] - x1;
                        y = ((-a[i - 1] / 6 * (x2 + h[i]) * x1 + yCoords[i - 1]) * x2 +
                            (-a[i] / 6 * (x1 + h[i]) * x2 + yCoords[i]) * x1) / h[i];
                        x = xCoords[i - 1] + x1;

                        output[count].X = x;
                        output[count].Y = y;
                        if (output[count].Y < minVal) { output[count].Y = minVal; }
                        else if (output[count].Y > maxVal) { output[count].Y = maxVal; }

                        if (start > 0 && i <= start)
                        {
                            output[count].X = points[i].X;
                            output[count].Y = points[i].Y;
                            if (output[count].Y < minVal) { output[count].Y = minVal; }
                            else if (output[count].Y > maxVal) { output[count].Y = maxVal; }
                        }

                        count++;
                    }
                }

                //interpolate if there isn´t the same amount of points and filecount:
                if (npp - filecount - precision != 0)
                {
                    int part = filecount / (Math.Abs(npp - filecount - precision) + 1);
                    PointF[] tmpOut = new PointF[output.Length];
                    output.CopyTo(tmpOut, 0);
                    int nr = 0;

                    for (int i = 0; i < filecount; i++)
                    {
                        if (i == part * (nr + 1) && nr < Math.Abs(npp - filecount - precision))
                        {
                            output[i].X = tmpOut[i - nr].X + ((tmpOut[i - nr].X - tmpOut[i - nr + 1].X) / 2);
                            output[i].Y = tmpOut[i - nr].Y + ((tmpOut[i - nr].Y - tmpOut[i - nr + 1].Y) / 2);
                            nr++;
                        }
                        else if (nr < Math.Abs(npp - filecount - precision))
                        {
                            output[i] = tmpOut[i - nr];
                        }
                        else
                        {
                            output[i] = tmpOut[i - nr + 1];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < filecount; i++)
                {
                    output[i].Y = points[0].Y;
                    output[i].X = points[0].X;
                }
            }
        }
    }
}