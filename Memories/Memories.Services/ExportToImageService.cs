using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Services
{
    public class ExportToImageService : IExportToImageService
    {
        public void ExportBookToImage(Book book, string path)
        {
            ImageFormat format = ImageFormat.JPEG;

            switch (Path.GetExtension(path))
            {
                case ".png":
                    format = ImageFormat.PNG;
                    break;

                case ".bmp":
                    format = ImageFormat.BMP;
                    break;
            }

            ExportBookToImage(book, format, path);
        }

        public void VisualToImage(object visual, BookUIPoint pixelSize, ImageFormat format, string path)
        {
            if (!(visual is Visual target))
            {
                throw new ArgumentException(visual.GetType().FullName + "is not Visual!" + Environment.NewLine + "In " + nameof(IExportToImageService));
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

            using (var fs = File.OpenWrite(path))
            {
                bitmapEncoder.Save(fs);
            }
        }

        private void ExportBookToImage(Book book, ImageFormat format, string path)
        {
            // Generate new folder
            string folderPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            Directory.CreateDirectory(folderPath);

            BookUIPoint imageSize = new BookUIPoint(book.PaperSize.GetWidth() * 10, book.PaperSize.GetHeight() * 10);

            List<BookPage> pages = new List<BookPage>(book.BookPages);
            pages.Insert(0, book.FrontCover);
            pages.Add(book.BackCover);

            for (int i = 0, l = pages.Count; i < l; i++)
            {
                BookPage page = pages[i];

                string imagePath = Path.Combine(folderPath, i + Path.GetExtension(path));
                VisualToImage(page.ToCanvas(book.PaperSize), imageSize, format, imagePath);
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
