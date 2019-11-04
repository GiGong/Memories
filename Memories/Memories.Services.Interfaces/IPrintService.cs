namespace Memories.Services.Interfaces
{
    public interface IPrintService
    {
        /// <summary>
        /// Print document
        /// </summary>
        /// <param name="document">Must be System.Windows.Documents.DocumentPaginator</param>
        /// <see cref="System.Windows.Documents.DocumentPaginator"/>
        void Print(object document);
    }
}
