using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Controls
{
    /// <summary>
    /// Interaction logic for InputDialogWindow.xaml
    /// </summary>
    public partial class InputDialogWindow : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");

        private bool _isNum;

        public bool IsNum
        {
            get => _isNum;
            set
            {
                _isNum = value;
                if (IsNum)
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                }
                else
                {
                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                }
            }
        }

        public InputDialogWindow()
        {
            InitializeComponent();

            IsNum = false;
            textBox.Focus();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public static string Show(string message)
        {
            return Show(message, null, MessageBoxImage.None, false);
        }

        public static string Show(string message, MessageBoxImage image)
        {
            return Show(message, null, image, false);
        }

        public static string Show(string message, bool isNum)
        {
            return Show(message, null, MessageBoxImage.None, isNum);
        }

        public static string Show(string message, string title, bool isNum)
        {
            return Show(message, title, MessageBoxImage.None, isNum);
        }

        public static string Show(string message, string title, MessageBoxImage image, bool isNum)
        {
            var window = new InputDialogWindow() { Title = title };
            window.message.Text = message;
            window.icon.Source = GetSourceFromIcon(image);
            window.IsNum = isNum;
            if (window.ShowDialog() == true)
            {
                return window.textBox.Text;
            }
            return null;
        }

        private static ImageSource GetSourceFromIcon(MessageBoxImage image)
        {
            IntPtr icon;
            switch (image)
            {
                case MessageBoxImage.Hand:
                    icon = SystemIcons.Hand.Handle;
                    break;
                case MessageBoxImage.Question:
                    icon = SystemIcons.Question.Handle;
                    break;
                case MessageBoxImage.Exclamation:
                    icon = SystemIcons.Exclamation.Handle;
                    break;
                case MessageBoxImage.Asterisk:
                    icon = SystemIcons.Asterisk.Handle;
                    break;
                default:
                    return null;
            }

            return Imaging.CreateBitmapSourceFromHIcon(icon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
