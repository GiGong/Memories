using Memories.Business.Converters;
using Newtonsoft.Json;
using System.IO;

namespace Memories.Business.IO
{
    public static class FileSystem
    {
        public static void SaveToJson<T>(T model, string path)
        {
            var json = JsonConvert.SerializeObject(model);

            File.WriteAllText(path, json);
        }

        public static T LoadFromJson<T>(string path)
        {
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json, new BookUIConverter());
        }

    }
}
