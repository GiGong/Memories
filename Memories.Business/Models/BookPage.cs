using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookPage : BusinessBase
    {
        private byte[] _background;
        public byte[] Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
        }

        private ObservableCollection<BookUI> _pageControls;
        public ObservableCollection<BookUI> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }
    }
}
