using Memories.Business.Enums;
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

        public Book MakeBook(string title, string writer, PaperSize paperSize, string path)
        {
            return new Book(title, writer, paperSize, path);
        }
    }
}
