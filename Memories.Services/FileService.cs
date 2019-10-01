using Memories.Services.Interfaces;
using Microsoft.Win32;

namespace Memories.Services
{
    public class FileService : IFileService
    {
        public string OpenFilePath(string filter = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = filter ?? "Memories Book|*.mrbk"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public string SaveFilePath(string filter = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = filter ?? "Memories Book|*.mrbk"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
