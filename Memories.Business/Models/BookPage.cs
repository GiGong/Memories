using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookPage : BusinessBase
    {
        private ObservableCollection<BookUI> _pageControls;
        public ObservableCollection<BookUI> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }
    }
}
