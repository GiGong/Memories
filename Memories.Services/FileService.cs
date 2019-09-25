using Memories.Services.Interfaces;
using Microsoft.Win32;

namespace Memories.Services
{
    public class FileService : IFileService
    {
        public string GetSaveFilePath()
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
