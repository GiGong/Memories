﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Memories.Core.Controls
{
    public class MMCenterImage : ContentControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Dependency Property

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MMCenterImage), new PropertyMetadata());

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MMCenterImage), new PropertyMetadata());


        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

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
            img.MouseLeftButtonDown += Image_MouseLeftButtonDown;

            Image = img;

            Content = Image;
        }

        #endregion Constructor

        #region Method

        public static explicit operator Image(MMCenterImage control)
        {
            return control.Image;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && Command != null)
            {
                if (Command.CanExecute(this))
                {
                    Command.Execute(this);
                    e.Handled = true;
                }
            }
        }

        #endregion Method
    }
}