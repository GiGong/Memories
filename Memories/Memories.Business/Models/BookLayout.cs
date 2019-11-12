using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookLayout : BusinessBase
    {
        #region Field

        private string _name;
        private byte[] _previewSource;
        private ObservableCollection<BookPage> _pages;

        private BookPage _frontCover;
        private BookPage _backCover;

        #endregion Field

        #region Property

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public byte[] PreviewSource
        {
            get { return _previewSource; }
            set { SetProperty(ref _previewSource, value); }
        }

        public ObservableCollection<BookPage> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }

        public BookPage FrontCover
        {
            get { return _frontCover; }
            set { SetProperty(ref _frontCover, value); }
        }

        public BookPage BackCover
        {
            get { return _backCover; }
            set { SetProperty(ref _backCover, value); }
        }

        #endregion Property

        #region Constructor

        public BookLayout()
        {
            Name = string.Empty;
            PreviewSource = null;
            Pages = new ObservableCollection<BookPage>();
            FrontCover = new BookPage();
            BackCover = new BookPage();
        }

        #endregion Constructor
    }
}
