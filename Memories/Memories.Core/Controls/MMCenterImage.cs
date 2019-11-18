using Memories.Business;
using System;
using System.ComponentModel;
using System.IO;
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
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MMCenterImage), new PropertyMetadata(null));

        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register(nameof(DropCommand), typeof(ICommand), typeof(MMCenterImage), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(MMCenterImage), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
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
                AllowDrop = true,
                DataContext = this
            };
            img.SetBinding(Image.SourceProperty, new Binding("ImageSource") { Mode = BindingMode.TwoWay });
            img.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            img.DragOver += Imgage_DragOver;
            img.Drop += Imgage_Drop;
            Image = img;

            Content = Image;
            Cursor = Cursors.Hand;
        }

        #endregion Constructor

        #region Method

        public static explicit operator Image(MMCenterImage control)
        {
            return control.Image;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Command != null && Command.CanExecute(this))
            {
                Command.Execute(this);
                e.Handled = true;
            }
        }

        private void Imgage_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (paths.Length != 1
                    || Array.IndexOf(ExtentionNames.IMAGE_EXTENSION_NAMES, Path.GetExtension(paths[0])) < 0)
                {
                    e.Effects = DragDropEffects.None;
                    // Block Drag, Drop
                    e.Handled = true;
                    return;
                }

                e.Effects = DragDropEffects.Copy;
            }
        }

        private void Imgage_Drop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            object[] parameter = new object[] { this, path };
            if (DropCommand != null && DropCommand.CanExecute(parameter))
            {
                DropCommand.Execute(parameter);
            }
        }

        #endregion Method
    }
}
