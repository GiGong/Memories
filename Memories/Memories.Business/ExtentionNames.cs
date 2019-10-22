namespace Memories.Business
{
    public static class ExtentionNames
    {
        public const string Book = "mrbk";
        public const string BookLayoutTemplate = "mrtl";
        public const string BookPageLayoutTemplate = "mrptl";
    }

    public static class ExtentionFilters
    {
        public const string Book = "Memories Book Files|*." + ExtentionNames.Book;
        public const string BookLayoutTemplate = "Memories Book Layout Files|*." + ExtentionNames.BookLayoutTemplate;
        public const string BookPageLayoutTemplate = "Memories Page Layout  Files|*." + ExtentionNames.BookPageLayoutTemplate;

        public const string ImageFiles = "Image Files|*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.bmp;*.dib;*.gif";
        public const string JPEG = "JPEG Files|*.jpg;*.jpeg";
        public const string PNG = "PNG Files|*.png";
        public const string BMP = "BMP Files|*.bmp";
    }
}
