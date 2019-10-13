using Memories.Business.Enums;
using Memories.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Services
{
    public class ExportToImageService : IExportToImageService
    {
        public void VisualToImage(object visual, Business.Models.Point pixelSize, ImageFormat format, string path)
        {
            if (!(visual is Visual target))
            {
                throw new ArgumentException(visual.GetType().FullName + "is not Visual!\nIn " + nameof(ExportToImageService));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)pixelSize.X, (int)pixelSize.Y, 96d, 96d, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Point(pixelSize.X, pixelSize.Y)));
            }
            rtb.Render(dv);


            BitmapEncoder bitmapEncoder = GetEncoderFromFormat(format);
            bitmapEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite(path))
            {
                bitmapEncoder.Save(fs);
            }
        }

        private BitmapEncoder GetEncoderFromFormat(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.JPEG:
                    return new JpegBitmapEncoder();
                case ImageFormat.PNG:
                    return new PngBitmapEncoder();
                case ImageFormat.BMP:
                    return new BmpBitmapEncoder();

                default:
                    throw new ArgumentOutOfRangeException(format + "is not enum ImageFormat");
            }
        }
    }
}
