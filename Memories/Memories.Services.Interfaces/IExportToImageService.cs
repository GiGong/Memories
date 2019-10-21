using Memories.Business.Enums;
using Memories.Business.Models;

namespace Memories.Services.Interfaces
{
    public interface IExportToImageService
    {
        /// <summary>
        /// Visual Export to Image file
        /// </summary>
        /// <param name="visual">Must be System.Windows.Media.Visual</param>
        /// <param name="printSize">pixel size to print</param>
        /// <param name="format">Jpeg, Png ... etc</param>
        void VisualToImage(object visual, BookUIPoint pixelSize, ImageFormat format, string path);
    }
}
