using Memories.Business;
using Memories.Business.Converters;
using Memories.Business.Models;
using Memories.Services.Interfaces;
using Newtonsoft.Json;
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
                if (Path.GetExtension(file) != ExtensionNames.BookLayoutTemplate)
                {
                    continue;
                }

                BookLayout layout = LoadFromFile(file);
                if (layout != null)
                {
                    layouts.Add(layout);
                }
            }

            return layouts;
        }

        private BookLayout LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<BookLayout>(json, new BookUIConverter());
        }
    }
}
