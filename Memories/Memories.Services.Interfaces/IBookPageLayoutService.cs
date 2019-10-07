using Memories.Business.Models;
using System.Collections.Generic;

namespace Memories.Services.Interfaces
{
    public interface IBookPageLayoutService
    {
        IEnumerable<BookPageLayout> LoadPageLayoutsFromDirectory(string path);
    }
}
