using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookPage : BusinessBase
    {
        private byte[] _background;        
        private ObservableCollection<BookUI> _pageControls;

        public byte[] Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
        }

        public ObservableCollection<BookUI> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }

        public BookPage()
        {
            Background = null;
            PageControls = new ObservableCollection<BookUI>();
        }

        public BookPage Clone()
        {
            return new BookPage()
            {
                Background = (byte[])Background?.Clone(),
                PageControls = new ObservableCollection<BookUI>(PageControls)
            };
        }
    }
}
