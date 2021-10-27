using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CandyMotion
{
    public static class Extensions
    {
        public static WriteableBitmap ToGrayScale(this WriteableBitmap frame)
        {
            for (int i = 0; i < frame.Pixels.Length; i++)
            {
                var px = frame.Pixels[i];
                var a = (byte)(px >> 24);
                var r = (byte)(px >> 16);
                var g = (byte)(px >> 8);
                var b = (byte)(px);

                byte gray = (byte)((r * 0.3) + (g * 0.59) + (b * 0.11));
                frame.Pixels[i] = (a << 24) | (gray << 16) | (gray << 8) | gray;
            }
            return frame;
        }

        public static int ToArgb(this Color color)
        {
            int argb = color.A << 24;
            argb += color.R << 16;
            argb += color.G << 8;
            argb += color.B;

            return argb;
        }
    }
}
