using Memories.Business.Enums;
using Memories.Business.Models;

namespace Memories.Services.Interfaces
{
    public interface IExportToImageService
    {
        void ExportBookToImage(Book book, string path);

        /// <summary>
        /// Visual Export to Image file
        /// </summary>
        /// <param name="visual">Must be System.Windows.Media.Visual</param>
        /// <param name="pixelSize">pixel size to print</param>
        /// <param name="format">Jpeg, Png ... etc</param>
        /// <param name="path">Path to save</param>
        void VisualToImage(object visual, BookUIPoint pixelSize, ImageFormat format, string path);
    }
}
