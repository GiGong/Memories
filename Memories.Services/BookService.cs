using Memories.Business.Converters;
using Memories.Business.Models;
using Memories.Services.Interfaces;
using Newtonsoft.Json;
using System.IO;

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
            var json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<Book>(json, new BookUIConverter());
        }

        public void SaveBook(Book book)
        {
            var json = JsonConvert.SerializeObject(book);

            File.WriteAllText(book.Path, json);
        }
    }
}
