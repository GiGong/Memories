using Memories.Business.Models;
using Memories.Core.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Extensions
{
    public static class BookExtension
    {
        // https://stackoverflow.com/questions/10138948/how-to-print-multiple-wpf-pages-in-one-document?rq=1
        public static FixedDocument ToFixedDocument(this Book book)
        {
            var pageSize = new Size(book.PaperSize.GetWidthPixel(), book.PaperSize.GetHeightPiexl());
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;

            List<BookPage> pages = new List<BookPage>(book.BookPages);
            pages.Insert(0, book.FrontCover);
            pages.Add(book.BackCover);

            foreach (var page in pages)
            {
                var fixedPage = new FixedPage
                {
                    Width = pageSize.Width,
                    Height = pageSize.Height
                };

                var visual = page.ToCanvas(book.PaperSize);
                if (page.Background != null)
                {
                    visual.Background = new ImageBrush(Convert(page.Background));
                }
                fixedPage.Children.Add(visual);

                //이미 ToCanvas() 내부에서 처리해줌.
                //fixedPage.Measure(pageSize);
                //fixedPage.Arrange(new Rect(new Point(), pageSize));
                //fixedPage.UpdateLayout();

                for (int i = 0, l = visual.Children.Count; i < l; i++)
                {
                    if (visual.Children[i] is MMCenterImage image)
                    {
                        BindingOperations.ClearBinding(image, MMCenterImage.ImageSourceProperty);
                        image.ImageSource = Convert(((BookImageUI)page.PageControls[i]).ImageSource);
                    }
                }

                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }

            return document;
        }

        // why.... 왜....
        // 이미지가 자기 영역을 넘어서 다른곳까지 침범하면 이거를 해줘야 함...
        // https://stackoverflow.com/a/36457610/7990500
        private static BitmapSource Convert(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            using (var ms = new MemoryStream(data))
            {
                return BitmapFrame.Create(
                    BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad));
            }
        }
    }
}
