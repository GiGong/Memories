using Memories.Core.Converters;
using Prism.Mvvm;
using System;
using System.Net;
using System.Windows.Media.Imaging;

namespace Memories.Modules.SelectImage
{
    public class ImageParameter : BindableBase
    {
        private byte[] _source;
        private string _preview;

        public byte[] Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        public string Preview
        {
            get { return _preview; }
            set { SetProperty(ref _preview, value); }
        }

        public void SetSourceFromPath(string path)
        {
            Preview = path;
            Source = ByteArrayToImageSourceConverter.SourceToByteArray(new BitmapImage(new Uri(path, UriKind.Relative)));
        }

        public void SetSourceFromUrl(string url)
        {
            Preview = url;
            Source = new WebClient().DownloadData(url);
        }
    }
}
