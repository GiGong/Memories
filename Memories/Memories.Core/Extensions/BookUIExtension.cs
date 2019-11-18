using Memories.Business.Models;
using Memories.Core.Controls;
using Memories.Core.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memories.Core.Extensions
{
    public static class BookUIExtension
    {
        public static MMRichTextBox ToRichTextBox(this BookTextUI bookTextUI, bool isLayout = false)
        {
            MMRichTextBox richTextBox = new MMRichTextBox();

            BookUIToFE(bookTextUI, richTextBox);
            richTextBox.SetBinding(MMRichTextBox.DataProperty,
                new Binding("Document")
                { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            return richTextBox;
        }

        public static BookTextUI ToBookTextUI(this MMRichTextBox richTextBox, bool isLayout = false)
        {
            var bookTextUI = new BookTextUI();

            FEToBookUI(richTextBox, bookTextUI);
            bookTextUI.Document = isLayout ? null : richTextBox.Data;

            return bookTextUI;
        }

        public static MMCenterImage ToImage(this BookImageUI bookImageUI, bool isLayout = false)
        {
            MMCenterImage image = new MMCenterImage();

            BookUIToFE(bookImageUI, image);
            if (!isLayout)
            {
                image.SetBinding(MMCenterImage.ImageSourceProperty,
                    new Binding("ImageSource")
                    {
                        Mode = BindingMode.TwoWay,
                        Converter = new ByteArrayToImageSourceConverter(),
                        TargetNullValue = new BitmapImage(new Uri("pack://application:,,,/Resources/Img/MemoriesEmptyImage.jpg"))
                    });
            }

            return image;
        }

        public static BookImageUI ToBookImageUI(this Image image, bool isLayout = false)
        {
            var bookImageUI = new BookImageUI();

            FEToBookUI(image, bookImageUI);
            bookImageUI.ImageSource = isLayout ? null : GetSourceFromImage((BitmapSource)image.Source);

            return bookImageUI;
        }

        public static void GetPropertyFromRectangle(this BookUI bookUI, Rectangle rect)
        {
            bookUI.Width = rect.Width;
            bookUI.Height = rect.Height;
            bookUI.Margin = new BookUIPoint(Canvas.GetLeft(rect), Canvas.GetTop(rect));
            bookUI.ZIndex = Panel.GetZIndex(rect);
            bookUI.Transform = rect.RenderTransform.Value.ToBookUIMatrix();
        }

        /// <summary>
        /// BookUI to FrameworkElement
        /// </summary>
        /// <param name="bookUI">From</param>
        /// <param name="element">To</param>
        private static void BookUIToFE(BookUI bookUI, FrameworkElement element)
        {
            element.SetBinding(FrameworkElement.WidthProperty, "Width");
            element.SetBinding(FrameworkElement.HeightProperty, "Height");

            element.SetBinding(Canvas.LeftProperty, "Margin.X");
            element.SetBinding(Canvas.TopProperty, "Margin.Y");

            element.SetBinding(Panel.ZIndexProperty, "ZIndex");

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.SetBinding(UIElement.RenderTransformProperty,
                new Binding("Transform")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new BookUIMatrixToTransformConverter()
                });

            element.DataContext = bookUI;
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

            bookUI.Margin = new BookUIPoint()
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
            return source == null ? null : ByteArrayToImageSourceConverter.SourceToByteArray(source);
        }
    }
}
