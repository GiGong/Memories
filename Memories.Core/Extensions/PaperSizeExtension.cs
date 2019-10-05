using Memories.Business.Enums;
using System;
using System.Drawing;

namespace Memories.Core.Extensions
{
    public static class PaperSizeExtension
    {
        public static double GetWidthPixel(this PaperSize paperSize)
        {
            return MilimeterToPixel(paperSize.GetWidth());
        }

        public static double GetHeightPiexl(this PaperSize paperSize)
        {
            return MilimeterToPixel(paperSize.GetHeight());
        }

        private static int MilimeterToPixel(double millimeter)
        {
            double pixel = -1;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                pixel = millimeter * g.DpiY / 25.4d;
            }
            return (int)Math.Round(pixel);
        }
    }
}
