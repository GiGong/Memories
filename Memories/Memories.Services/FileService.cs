using Memories.Business;
using Memories.Services.Interfaces;
using Microsoft.Win32;

namespace Memories.Services
{
    public class FileService : IFileService
    {
        public string OpenFilePath(string filter = null, string title = "Memories")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter ?? ExtentionFilters.Book
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public string SaveFilePath(string filter = null, string title = "Memories")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = filter ?? ExtentionFilters.Book
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
