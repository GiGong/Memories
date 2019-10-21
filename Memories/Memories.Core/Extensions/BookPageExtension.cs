using Memories.Business.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Memories.Core.Extensions
{
    public static class BookPageExtension
    {
        public static ObservableCollection<UIElement> ToUIElementCollection(this BookPage bookPage)
        {
            return new ObservableCollection<UIElement>(bookPage.PageControls.Select(BookUIToUIElement));
        }

        private static UIElement BookUIToUIElement(BookUI source)
        {
            if (source.UIType == Business.Enums.BookUIEnum.TextUI)
            {
                var richTextBox = (source as BookTextUI).ToRichTextBox();

                Xceed.Wpf.Toolkit.RichTextBoxFormatBarManager.SetFormatBar(richTextBox, new Xceed.Wpf.Toolkit.RichTextBoxFormatBar() { Width = 200, Height = 75 });
                return richTextBox;
            }
            else if (source.UIType == Business.Enums.BookUIEnum.ImageUI)
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
