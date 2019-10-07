using System.Collections.Generic;
using System.IO;
using Memories.Business;
using Memories.Business.IO;
using Memories.Business.Models;
using Memories.Services.Interfaces;

namespace Memories.Services
{
    public class BookPageLayoutService : IBookPageLayoutService
    {
        public IEnumerable<BookPageLayout> LoadPageLayoutsFromDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return new List<BookPageLayout>(0);
            }

            string[] files = Directory.GetFiles(path);
            List<BookPageLayout> layouts = new List<BookPageLayout>(files.Length);

            foreach (string file in files)
            {
                if (Path.GetExtension(file) != "." + ExtentionNames.BookPageLayoutTemplate)
                {
                    continue;
                }

                BookPageLayout layout = FileSystem.LoadFromJson<BookPageLayout>(file);
                if (layout != null)
                {
                    layouts.Add(layout);
                }
            }

            return layouts;
        }
    }
}
