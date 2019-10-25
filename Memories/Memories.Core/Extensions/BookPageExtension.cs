using Memories.Business.Enums;
using Memories.Business.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Extensions
{
    public static class BookPageExtension
    {
        public static ObservableCollection<UIElement> ToUIElementCollection(this BookPage bookPage)
        {
            ObservableCollection<UIElement> collection = new ObservableCollection<UIElement>();

            foreach (BookUI bookUI in bookPage.PageControls)
            {
                collection.Add(BookUIToUIElement(bookUI));
            }
            return collection;
        }

        public static Canvas ToCanvas(this BookPage bookPage, PaperSize paperSize)
        {
            double width = paperSize.GetWidthPixel();
            double height = paperSize.GetHeightPiexl();

            Canvas canvas = new Canvas()
            {
                Width = width,
                Height = height
            };

            canvas.Background = bookPage.Background == null
                ? (Brush)Brushes.White
                : new ImageBrush((BitmapSource)new ImageSourceConverter().ConvertFrom(bookPage.Background)) { Stretch = Stretch.Fill };


            foreach (UIElement element in bookPage.ToUIElementCollection())
            {
                canvas.Children.Add(element);
            }

            // Viewbox can render controls in code behind
            var viewbox = new Viewbox() { Child = canvas };
            viewbox.Measure(new Size(width, height));
            viewbox.Arrange(new Rect(0, 0, width, height));
            viewbox.UpdateLayout();
            
            return canvas;
        }

        private static UIElement BookUIToUIElement(BookUI source)
        {
            if (source.UIType == BookUIEnum.TextUI)
            {
                var richTextBox = (source as BookTextUI).ToRichTextBox();

                //Xceed.Wpf.Toolkit.RichTextBoxFormatBarManager.SetFormatBar(richTextBox, new Xceed.Wpf.Toolkit.RichTextBoxFormatBar() { Width = 250, Height = 100 });
                return richTextBox;
            }
            else if (source.UIType == BookUIEnum.ImageUI)
            {
                var image = (source as BookImageUI).ToImage();
                return image;
            }
            else
            {
                throw new ArgumentOutOfRangeException(source + " is not BookUI");
            }
        }
    }
}
