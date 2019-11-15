using Memories.Business.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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

                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }

            return document;
        }

        // https://stackoverflow.com/a/36457610/7990500
        private static BitmapSource Convert(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return BitmapFrame.Create(
                    BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad));
            }
        }
    }
}
