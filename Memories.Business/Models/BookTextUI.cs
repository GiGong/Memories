using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookTextUI : BookUI
    {
        private ObservableCollection<string> _documents;
        public ObservableCollection<string> Documents
        {
            get { return _documents; }
            set { SetProperty(ref _documents, value); }
        }
    }
}
