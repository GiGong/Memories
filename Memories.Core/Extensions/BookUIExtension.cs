using Memories.Business.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Extensions
{
    public static class BookUIExtension
    {
        public static Xceed.Wpf.Toolkit.RichTextBox ToRichTextBox(this BookTextUI bookTextUI)
        {
            Xceed.Wpf.Toolkit.RichTextBox richTextBox = new Xceed.Wpf.Toolkit.RichTextBox();

            BookUIToFE(bookTextUI, richTextBox);
            richTextBox.Text = bookTextUI.Document;

            return richTextBox;
        }

        public static BookTextUI ToBookTextUI(this Xceed.Wpf.Toolkit.RichTextBox richTextBox)
        {
            var bookTextUI = new BookTextUI();

            FEToBookUI(richTextBox, bookTextUI);
            bookTextUI.Document = richTextBox.Text;

            return bookTextUI;
        }

        public static Image ToImage(this BookImageUI bookImageUI)
        {
            Image image = new Image();

            BookUIToFE(bookImageUI, image);
            image.Source = GetSourceFromBook(bookImageUI);

            return image;
        }

        public static BookImageUI ToBookImageUI(this Image image)
        {
            var bookImageUI = new BookImageUI();

            FEToBookUI(image, bookImageUI);
            bookImageUI.ImageSource = GetSourceFromImage((BitmapSource)image.Source);

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
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        /// <summary>
        /// Return source from BookImageUI
        /// </summary>
        /// <param name="bookImageUI"></param>
        /// <returns></returns>
        private static BitmapSource GetSourceFromBook(BookImageUI bookImageUI)
        {
            return bookImageUI.ImageSource == null ? null : (BitmapSource)new ImageSourceConverter().ConvertFrom(bookImageUI.ImageSource);
        }
    }
}
