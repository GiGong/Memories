using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Memories.Core.Controls
{
    public class MMCenterImage : ContentControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Dependency Property

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MMCenterImage), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set
            {
                SetValue(ImageSourceProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
            }
        }

        #endregion Dependency Property


        public Image Image { get; }


        #region Constructor

        public MMCenterImage()
        {
            var img = new Image()
            {
                Stretch = Stretch.UniformToFill,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                DataContext = this
            };
            img.SetBinding(Image.SourceProperty, new Binding("ImageSource") { Mode = BindingMode.TwoWay });
            Image = img;

            Content = Image;
        }

        #endregion Constructor

        #region Method

        public static explicit operator Image(MMCenterImage control)
        {
            return control.Image;
        }

        #endregion Method
    }
}
