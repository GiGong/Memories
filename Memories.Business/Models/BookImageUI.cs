using System.Collections.ObjectModel;
using System.IO;

namespace Memories.Business.Models
{
    public class BookImageUI : BookUI
    {
        private ObservableCollection<Stream> _imageSources;
        public ObservableCollection<Stream> ImageSources
        {
            get { return _imageSources; }
            set { SetProperty(ref _imageSources, value); }
        }
    }
}
