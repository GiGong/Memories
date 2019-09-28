using Memories.Services.Interfaces;
using Microsoft.Win32;

namespace Memories.Services
{
    public class FileService : IFileService
    {
        public string OpenFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Memories Book|*.mrbk"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public string SaveFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Memories Book|*.mrbk"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
