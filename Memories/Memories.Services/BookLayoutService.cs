using Memories.Business;
using Memories.Business.IO;
using Memories.Business.Models;
using Memories.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Memories.Services
{
    public class BookLayoutService : IBookLayoutService
    {
        public IEnumerable<BookLayout> LoadLayoutsFromDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return new List<BookLayout>(0);
            }

            string[] files = Directory.GetFiles(path);
            List<BookLayout> layouts = new List<BookLayout>(files.Length);

            foreach (string file in files)
            {
                if (Path.GetExtension(file) != "." + ExtentionNames.BookLayoutTemplate)
                {
                    continue;
                }

                BookLayout layout = FileSystem.LoadFromJson<BookLayout>(file);
                if (layout != null)
                {
                    layouts.Add(layout);
                }
            }

            return layouts;
        }
    }
}
