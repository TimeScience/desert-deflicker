using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DeSERt
{
    internal struct LinearBitmap
    {
        public static Bitmap CreateLinearBitmap(Bitmap InBmp, string Colorspace)
        {
            PixelFormat pf = InBmp.PixelFormat;
            Rectangle rec = new Rectangle(0, 0, InBmp.Width, InBmp.Height);
            BitmapData bmdIn = InBmp.LockBits(rec, ImageLockMode.ReadWrite, pf);
            Bitmap OutBmp = new Bitmap(InBmp.Width, InBmp.Height);
            BitmapData bmdOut = OutBmp.LockBits(rec, ImageLockMode.ReadWrite, pf);

            int PixelSize;
            if (pf == PixelFormat.Format16bppArgb1555 || pf == PixelFormat.Format16bppGrayScale || pf == PixelFormat.Format16bppRgb555 || pf == PixelFormat.Format16bppRgb565)
            { PixelSize = 2; }
            else if (pf == PixelFormat.Format24bppRgb)
            { PixelSize = 3; }
            else if (pf == PixelFormat.Format32bppArgb || pf == PixelFormat.Format32bppPArgb || pf == PixelFormat.Format32bppRgb)
            { PixelSize = 4; }
            else if (pf == PixelFormat.Format48bppRgb)
            { PixelSize = 6; }
            else if (pf == PixelFormat.Format64bppArgb || pf == PixelFormat.Format64bppPArgb)
            { PixelSize = 8; }
            else if (pf == PixelFormat.Format8bppIndexed)
            { PixelSize = 1; }
            else { throw new Exception("Wrong PixelFormat"); }

            int index = 0;
            double r = 0;
            double g = 0;
            double b = 0;

            unsafe
            {
                for (int y = 0; y < bmdIn.Height; y++)
                {
                    byte* rowIn = (byte*)bmdIn.Scan0 + (y * bmdIn.Stride);
                    byte* rowOut = (byte*)bmdOut.Scan0 + (y * bmdOut.Stride);

                    for (int x = 0; x < bmdIn.Width; x++)
                    {
                        index = x * PixelSize;
                        b = rowIn[index] / 255f;
                        g = rowIn[index + 1] / 255f;
                        r = rowIn[index + 2] / 255f;

                        if (Colorspace == "sRGB")
                        {
                            if (r <= 0.03928f) { r /= 12.92f; }
                            else { r = Math.Pow((r + 0.055f) / 1.055f, 2.4f); }

                            if (g <= 0.03928f) { g /= 12.92f; }
                            else { g = Math.Pow((g + 0.055f) / 1.055f, 2.4f); }

                            if (b <= 0.03928f) { b /= 12.92f; }
                            else { b = Math.Pow((b + 0.055f) / 1.055f, 2.4f); }
                        }
                        else
                        {
                            r = Math.Pow(r, 2.2f);
                            g = Math.Pow(g, 2.2f);
                            b = Math.Pow(b, 2.2f);
                        }

                        r *= 255;
                        g *= 255;
                        b *= 255;

                        if (r > 255) { r = 255; }
                        else if (r < 0) { r = 0; }
                        if (g > 255) { g = 255; }
                        else if (g < 0) { g = 0; }
                        if (b > 255) { b = 255; }
                        else if (b < 0) { b = 0; }

                        rowOut[index] = (byte)b;
                        rowOut[index + 1] = (byte)g;
                        rowOut[index + 2] = (byte)r;
                    }
                }
            }
            InBmp.UnlockBits(bmdIn);
            OutBmp.UnlockBits(bmdOut);
            return OutBmp;
        }
    }
}