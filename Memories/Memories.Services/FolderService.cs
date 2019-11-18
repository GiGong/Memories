using Memories.Services.Interfaces;
using System.IO;
using System.Reflection;

namespace Memories.Services
{
    public class FolderService : IFolderService
    {
        public string GetInstallationFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public string GetAppFolder(params string[] paths)
        {
            return Path.Combine(GetInstallationFolder(), Path.Combine(paths));
        }

        public string GetBookTemplateFolder(string paperSize)
        {
            return GetAppFolder("Templates", paperSize, "Book");
        }

        public string GetPageTemplateFolder(string paperSize)
        {
            return GetAppFolder("Templates", paperSize, "Page");
        }
    }
}
