using System.Collections.ObjectModel;
using System.IO;

namespace Memories.Business.Models
{
    public class BookLayout : BusinessBase
    {
        #region Field

        private string _name;
        private Stream _previewSource;
        private ObservableCollection<BookPage> _pages;

        #endregion Field

        #region Property

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public Stream PreviewSource
        {
            get { return _previewSource; }
            set { SetProperty(ref _previewSource, value); }
        }

        public ObservableCollection<BookPage> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }

        #endregion Property
    }
}
