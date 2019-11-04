using Memories.Business.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;

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

            foreach (var item in pages)
            {
                var fixedPage = new FixedPage
                {
                    Width = pageSize.Width,
                    Height = pageSize.Height
                };

                var visual = item.ToCanvas(book.PaperSize);
                fixedPage.Children.Add(visual);

                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }

            return document;
        }
    }
}
