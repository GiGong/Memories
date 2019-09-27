using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class BookTextUI : BookUI
    {
        private string _document;
        public string Document
        {
            get { return _document; }
            set { SetProperty(ref _document, value); }
        }
    }
}
