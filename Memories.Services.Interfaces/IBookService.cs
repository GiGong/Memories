using Memories.Business.Enums;
using Memories.Business.Models;

namespace Memories.Services.Interfaces
{
    public interface IBookService
    {
        Book GetEmptyBook();
        Book MakeBook(string title, string writer, PaperSize paperSize, string path);

    }
}
