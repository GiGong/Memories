using Memories.Business;
using Memories.Business.Models;
using Memories.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Memories.Services
{
    public class BookLayoutService : IBookLayoutService
    {
        public IEnumerable<BookLayout> LoadFromDirectory(string path)
        {
            //TODO: Remove this
            DebugFunction(path);


            //string[] files = Directory.GetFiles(path, "*." + ExtensionNames.BookLayoutTemplate);
            //List<BookLayout> layouts = new List<BookLayout>(files.Length);

            //foreach (string file in files)
            //{
            //    BookLayout layout = LoadFromFile(file);
            //    if (layout != null)
            //    {
            //        layouts.Add(layout);
            //    }
            //}


            return new List<BookLayout>() { new BookLayout { Name = "포토북" }, new BookLayout { Name = "SNS 북" } };
        }

        private BookLayout LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);

            
            
            JsonConvert.DeserializeObject<BookLayout>(json);

            return null;
        }

        private void DebugFunction(string path)
        {
            var template = new BookLayout { Name = "포토북" };




            string json = JsonConvert.SerializeObject(template);
        }
    }
}
