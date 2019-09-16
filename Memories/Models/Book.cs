using Memories.Enums;
using System;

namespace Memories.Models
{
    public class Book
    {
        #region Property

        public string Title { get; private set; }
        public string Writer { get; private set; }
        public PaperSize PaperSize { get; private set; }
        public string Path { get; private set; }
        public BookPage[] BookPages { get; private set; }

        #endregion Property

        #region Constructor

        public Book(string title, string writer, PaperSize paperSize, string path, int numOfPage = 5)
        {
            Title = title;
            Writer = writer;
            PaperSize = paperSize;
            Path = path;

            BookPages = new BookPage[numOfPage];
        }

        #endregion Constructor

        #region Method

        public void AddPage(int numOfPage)
        {
            BookPage[] newBookPages = new BookPage[BookPages.Length + numOfPage];
            BookPages.CopyTo(newBookPages, 0);

            BookPages = newBookPages;
        }

        public void RemovePage(int index)
        {
            throw new NotImplementedException();
        }

        #endregion Method
    }
}
