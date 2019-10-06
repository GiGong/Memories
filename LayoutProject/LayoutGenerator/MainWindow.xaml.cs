using Memories.Business;
using Memories.Business.Converters;
using Memories.Business.Models;
using Memories.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LayoutGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        readonly List<Canvas> _canvas = new List<Canvas>();

        private string _formatText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FormatText
        {
            get { return _formatText; }
            set
            {
                _formatText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FormatText"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _canvas.Add(pageCanvas1);
            _canvas.Add(pageCanvas2);
            //_canvas.Add(pageCanvas3);
            //_canvas.Add(pageCanvas4);

            FormatText = null;
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            BookLayout bookLayout = new BookLayout()
            {
                Name = textBox.Text,
                PreviewSource = GetSourceFromImage(imgPreview),
                Pages = new ObservableCollection<BookPage>()
            };

            foreach (var item in _canvas)
            {
                bookLayout.Pages.Add(new BookPage()
                {
                    PageControls = new ObservableCollection<BookUI>(GetBookUIsFromCanvas(item))
                });
            }

            string json = JsonConvert.SerializeObject(bookLayout);
            //string json = JsonConvert.SerializeObject(bookLayout, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Outputs", textBox.Text + ".json");

            File.WriteAllText(path, json);
        }

        private List<BookUI> GetBookUIsFromCanvas(Canvas canvas)
        {
            List<BookUI> bookUIs = new List<BookUI>();

            foreach (var item in canvas.Children)
            {
                if (item is Image image)
                {
                    bookUIs.Add(image.ToBookImageUI(true));
                }
                else if (item is Xceed.Wpf.Toolkit.RichTextBox richTextBok)
                {
                    bookUIs.Add(richTextBok.ToBookTextUI(true));
                }
            }

            return bookUIs;
        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Outputs", "Gen.json");
            string json = File.ReadAllText(path);

            BookLayout bookLayout = JsonConvert.DeserializeObject<BookLayout>(json, new BookUIConverter());
            //BookLayout bookLayout = JsonConvert.DeserializeObject<BookLayout>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });




        }

        private static byte[] GetSourceFromImage(Image image)
        {
            var source = image.Source as BitmapImage;
            if (source == null)
            {
                return null;
            }

            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
