using Memories.Business.IO;
using Memories.Business.Models;
using Memories.Services.Interfaces;

namespace Memories.Services
{
    public class BookService : IBookService
    {
        public Book GetEmptyBook()
        {
            return new Book();
        }

        public Book LoadBook(string path)
        {
            return FileSystem.LoadFromJson<Book>(path);
        }

        public void SaveBook(Book book, string path)
        {
            FileSystem.SaveToJson(book, path);
        }

        public void AddPage(Book book, BookPage page)
        {
            throw new System.NotImplementedException();
        }
    }
}
