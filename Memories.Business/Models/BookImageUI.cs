namespace Memories.Business.Models
{
    public class BookImageUI : BookUI
    {
        private byte[] _imageSource;
        public byte[] ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        public BookImageUI()
        {
            UIType = Enums.BookUIEnum.ImageUI;
        }
    }
}
