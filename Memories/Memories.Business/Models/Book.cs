using Memories.Business.Enums;
using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class Book : BusinessBase
    {
        #region Field

        private string _title;
        private string _writer;
        private PaperSize _paperSize;
        private double _paperWidth;
        private double _paperHeight;
        private ObservableCollection<BookPage> _bookPages;

        #endregion Field

        #region Property

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Writer
        {
            get { return _writer; }
            set { SetProperty(ref _writer, value); }
        }

        public PaperSize PaperSize
        {
            get { return _paperSize; }
            set
            {
                SetProperty(ref _paperSize, value);

                PaperWidth = PaperSize.GetWidth();
                PaperHeight = PaperSize.GetHeight();
            }
        }

        public double PaperWidth
        {
            get { return _paperWidth; }
            set { SetProperty(ref _paperWidth, value); }
        }

        public double PaperHeight
        {
            get { return _paperHeight; }
            set { SetProperty(ref _paperHeight, value); }
        }

        public ObservableCollection<BookPage> BookPages
        {
            get { return _bookPages; }
            set { SetProperty(ref _bookPages, value); }
        }

        #endregion Property

        #region Constructor

        public Book()
        {
            Title = string.Empty;
            Writer = string.Empty;
            PaperSize = PaperSize.비규격;
            PaperWidth = -1;
            PaperHeight = -1;
            BookPages = new ObservableCollection<BookPage>();
        }

        #endregion Constructor

        #region Method

        public Book Clone()
        {
            return new Book()
            {
                Title = Title,
                Writer = Writer,
                PaperSize = PaperSize,
                PaperWidth = PaperWidth,
                PaperHeight = PaperHeight,
                BookPages = new ObservableCollection<BookPage>(BookPages)
            };
        }

        #endregion Method
    }
}
