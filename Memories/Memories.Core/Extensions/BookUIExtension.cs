using Memories.Business.Models;
using Memories.Core.Controls;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Extensions
{
    public static class BookUIExtension
    {
        public static Xceed.Wpf.Toolkit.RichTextBox ToRichTextBox(this BookTextUI bookTextUI, bool isLayout = false)
        {
            Xceed.Wpf.Toolkit.RichTextBox richTextBox = new Xceed.Wpf.Toolkit.RichTextBox();

            BookUIToFE(bookTextUI, richTextBox);
            richTextBox.Text = isLayout ? null : bookTextUI.Document;

            return richTextBox;
        }

        public static BookTextUI ToBookTextUI(this Xceed.Wpf.Toolkit.RichTextBox richTextBox, bool isLayout = false)
        {
            var bookTextUI = new BookTextUI();

            FEToBookUI(richTextBox, bookTextUI);
            bookTextUI.Document = isLayout ? null : richTextBox.Text;

            return bookTextUI;
        }

        public static Image ToImage(this BookImageUI bookImageUI, bool isLayout = false)
        {
            MMClickableImage image = new MMClickableImage();

            BookUIToFE(bookImageUI, image);

            return image;
        }

        public static BookImageUI ToBookImageUI(this Image image, bool isLayout = false)
        {
            var bookImageUI = new BookImageUI();

            FEToBookUI(image, bookImageUI);
            bookImageUI.ImageSource = isLayout ? null : GetSourceFromImage((BitmapSource)image.Source);

            return bookImageUI;
        }

        /// <summary>
        /// BookUI to FrameworkElement
        /// </summary>
        /// <param name="bookUI">From</param>
        /// <param name="element">To</param>
        private static void BookUIToFE(BookUI bookUI, FrameworkElement element)
        {
            element.Width = bookUI.Width;
            element.Height = bookUI.Height;

            Canvas.SetLeft(element, bookUI.Margin.X);
            Canvas.SetTop(element, bookUI.Margin.Y);

            Panel.SetZIndex(element, bookUI.ZIndex);

            element.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

            element.RenderTransform = new MatrixTransform(bookUI.Transform.ToMatrix());
        }

        /// <summary>
        /// FrameworkElement to BookUI
        /// </summary>
        /// <param name="element">From</param>
        /// <param name="bookUI">To</param>
        private static void FEToBookUI(FrameworkElement element, BookUI bookUI)
        {
            bookUI.Width = element.Width;
            bookUI.Height = element.Height;

            bookUI.Margin = new Business.Models.Point()
            {
                X = Canvas.GetLeft(element),
                Y = Canvas.GetTop(element),
            };

            bookUI.ZIndex = Panel.GetZIndex(element);

            bookUI.Transform = element.RenderTransform.Value.ToBookUIMatrix();
        }

        /// <summary>
        /// Extract source from image control
        /// </summary>
        /// <param name="image">source image</param>
        /// <returns></returns>
        private static byte[] GetSourceFromImage(BitmapSource source)
        {
            if (source == null)
            {
                return null;
            }

            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
