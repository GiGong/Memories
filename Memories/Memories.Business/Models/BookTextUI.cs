namespace Memories.Business.Models
{
    public class BookTextUI : BookUI
    {
        private byte[] _document;

        public byte[] Document
        {
            get { return _document; }
            set { SetProperty(ref _document, value); }
        }

        public BookTextUI()
        {
            UIType = Enums.BookUIEnum.TextUI;
        }
    }
}
