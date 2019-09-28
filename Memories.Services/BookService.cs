using Memories.Business.Enums;
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

            JsonConvert.DeserializeObject<Book>(json);

            return null;
        }
    }
}
