using System.IO;

namespace Memories.Business.Models
{
    public class BookLayout : BookPage
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private Stream _previewSource;
        public Stream PreviewSource
        {
            get { return _previewSource; }
            set { SetProperty(ref _previewSource, value); }
        }
    }
}
