namespace Memories.Business.Models
{
    public class BookPageLayout : BusinessBase
    {
        #region Field

        private string _name;
        private byte[] _previewSource;
        private BookPage _page;

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

        public BookPage Page
        {
            get { return _page; }
            set { SetProperty(ref _page, value); }
        }

        #endregion Property
    }
}
