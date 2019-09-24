using System.Collections.ObjectModel;
using System.IO;

namespace Memories.Business.Models
{
    public class BookPage : BusinessBase
    {
        private ObservableCollection<string> _documents;
        public ObservableCollection<string> Documents
        {
            get { return _documents; }
            set { SetProperty(ref _documents, value); }
        }

        private ObservableCollection<Stream> _imageSources;
        public ObservableCollection<Stream> ImageSources
        {
            get { return _imageSources; }
            set { SetProperty(ref _imageSources, value); }
        }

        private ObservableCollection<BookUI> _pageControls;
        public ObservableCollection<BookUI> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }
    }
}
