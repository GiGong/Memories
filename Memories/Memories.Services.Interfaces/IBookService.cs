using Memories.Business.Enums;
using Memories.Business.Models;

namespace Memories.Services.Interfaces
{
    public interface IBookService
    {
        Book GetEmptyBook();
        Book LoadBook(string path);
        void SaveBook(Book book, string path);

        void AddPage(Book book, BookPage page);
    }
}
