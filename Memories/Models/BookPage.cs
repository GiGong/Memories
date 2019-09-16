using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Memories.Models
{
    public class BookPage
    {
        public string Documents { get; private set; }
        public BitmapImage[] ImageSources { get; private set; }

        public ObservableCollection<UIElement> PageControls { get; private set; }
    }
}
