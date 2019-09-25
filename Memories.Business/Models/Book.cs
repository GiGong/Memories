using Memories.Business.Enums;
using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class Book : BusinessBase
    {
        #region Property

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _writer;
        public string Writer
        {
            get { return _writer; }
            set { SetProperty(ref _writer, value); }
        }

        private PaperSize _paperSize;
        public PaperSize PaperSize
        {
            get { return _paperSize; }
            set { SetProperty(ref _paperSize, value); }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        private ObservableCollection<BookPage> _bookPages;
        public ObservableCollection<BookPage> BookPages
        {
            get { return _bookPages; }
            set { SetProperty(ref _bookPages, value); }
        }

        #endregion Property

        #region Constructor

        public Book() { }

        public Book(string title, string writer, PaperSize paperSize, string path)
        {
            Title = title;
            Writer = writer;
            PaperSize = paperSize;
            Path = path;

            BookPages = new ObservableCollection<BookPage>();
        }

        #endregion Constructor
    }
}
