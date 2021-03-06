﻿using Memories.Business;
using Memories.Business.Converters;
using Memories.Business.Models;
using Memories.Core.Controls;
using Memories.Core.Extensions;
using Microsoft.Win32;
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
        readonly List<Canvas> _canvas;

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

            _canvas = new List<Canvas>(gridCanvas.Children.Count);
            foreach (var item in gridCanvas.Children)
            {
                if (item is Canvas canvas)
                {
                    _canvas.Add(canvas);
                }
            }
            //_canvas.Add(pageCanvas1);
            //_canvas.Add(pageCanvas2);
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
                PreviewSource = GetSourceFromImage(imgPreview.Source),
                Pages = new ObservableCollection<BookPage>(),
                FrontCover = new BookPage()
                {
                    Background = coverCanvas1.Background is ImageBrush image1 ? GetSourceFromImage(image1.ImageSource) : null,
                    PageControls = new ObservableCollection<BookUI>(GetBookUIsFromCanvas(coverCanvas1))
                },
                BackCover = new BookPage()
                {
                    Background = coverCanvas2.Background is ImageBrush image2 ? GetSourceFromImage(image2.ImageSource) : null,
                    PageControls = new ObservableCollection<BookUI>(GetBookUIsFromCanvas(coverCanvas2))
                }
            };
            SavePageLayout(bookLayout.FrontCover, coverCanvas1.Tag?.ToString());
            SavePageLayout(bookLayout.BackCover, coverCanvas2.Tag?.ToString());

            foreach (var item in _canvas)
            {
                var page = new BookPage()
                {
                    Background = item.Background is ImageBrush image ? GetSourceFromImage(image.ImageSource) : null,
                    PageControls = new ObservableCollection<BookUI>(GetBookUIsFromCanvas(item))
                };
                bookLayout.Pages.Add(page);
                SavePageLayout(page, item.Tag?.ToString());
            }

            string json = JsonConvert.SerializeObject(bookLayout);

            string path = Path.Combine(@"W:\Data\Outputs\BookLayout", BookSize, textBox.Text + ".mrtl");

            File.WriteAllText(path, json);
        }
        private const string BookSize = "A5";
        private void SavePageLayout(BookPage bookPage, string name)
        {
            if (bookPage.Background == null || string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            BookPageLayout bookPageLayout = new BookPageLayout()
            {
                Name = name,
                PreviewSource = bookPage.Background,
                Page = bookPage
            };

            string json = JsonConvert.SerializeObject(bookPageLayout);
            string path = Path.Combine(@"W:\Data\Outputs\PageLayout", BookSize, name + ".mrptl");

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
                else if (item is MMRichTextBox richTextBok)
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

            BookLayout bookLayout = JsonConvert.DeserializeObject<BookLayout>(json, new BookUIJsonConverter());
            //BookLayout bookLayout = JsonConvert.DeserializeObject<BookLayout>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
        }

        private static byte[] GetSourceFromImage(ImageSource source)
        {
            if (source == null)
            {
                return null;
            }

            var image = source as BitmapSource;

            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void imgPreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    imgPreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            }
        }

        private void SavePage_Click(object sender, RoutedEventArgs e)
        {
            BookPageLayout bookPageLayout = new BookPageLayout()
            {
                Name = textBox.Text,
                PreviewSource = GetSourceFromImage(imgPreview.Source),
                Page = new BookPage()
                {
                    Background = pageCanvas1.Background is ImageBrush image ? GetSourceFromImage(image.ImageSource) : null,
                    PageControls = new ObservableCollection<BookUI>(GetBookUIsFromCanvas(pageCanvas1))
                }
            };


            string json = JsonConvert.SerializeObject(bookPageLayout);
            //string json = JsonConvert.SerializeObject(bookLayout, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Outputs", textBox.Text + ".mrptl");

            File.WriteAllText(path, json);
        }

        private void LoadPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {

            _ = imgPreview.RenderTransform.Value;
        }
    }
}
