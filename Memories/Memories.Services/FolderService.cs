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
    }
}
