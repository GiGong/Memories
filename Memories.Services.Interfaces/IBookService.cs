using Memories.Business.Enums;
using Memories.Business.Models;

namespace Memories.Services.Interfaces
{
    public interface IBookService
    {
        Book GetEmptyBook();
        Book LoadBook(string path);
    }
}
