using Memories.Business.Models;
using System.Collections.Generic;

namespace Memories.Services.Interfaces
{
    public interface IBookLayoutService
    {
        /// <summary>
        /// Load templates from folder
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns></returns>
        IEnumerable<BookLayout> LoadLayoutsFromDirectory(string path);
    }
}
