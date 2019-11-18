namespace Memories.Business
{
    public static class ExtentionNames
    {
        public const string Book = "mrbk";
        public const string BookLayoutTemplate = "mrtl";
        public const string BookPageLayoutTemplate = "mrptl";

        public const string JPG = ".jpg";
        public const string JPEG = ".jpeg";
        public const string JPE = ".jpe";
        public const string JFIF = ".jfif";
        public const string PNG = ".png";
        public const string BMP = ".bmp";
        public const string DIB = ".dib";
        public const string GIF = ".gif";

        public static readonly string[] IMAGE_EXTENSION_NAMES = new string[] { JPG, JPEG, JPE, JFIF, PNG, BMP, DIB, GIF };
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
