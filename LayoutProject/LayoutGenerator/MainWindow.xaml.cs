using Memories.Business.Models;
using Memories.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Shapes;

namespace LayoutGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BookLayout bookLayout = new BookLayout()
            {
                Name = "Test Template",
                Pages = new ObservableCollection<BookPage>()
            };

            bookLayout.Pages.Add(new BookPage()
            {
                PageControls = new ObservableCollection<BookUI>(GetBookUIFromCanvas(pageCanvas1))
            });

            bookLayout.Pages.Add(new BookPage()
            {
                PageControls = new ObservableCollection<BookUI>(GetBookUIFromCanvas(pageCanvas2))
            });

            string json = JsonConvert.SerializeObject(bookLayout);
        }

        private List<BookUI> GetBookUIFromCanvas(Canvas canvas)
        {
            List<BookUI> bookUIs = new List<BookUI>();

            foreach (var item in canvas.Children)
            {
                if (item is Image image)
                {
                    bookUIs.Add(image.ToBookImageUI());
                }
                else if (item is Xceed.Wpf.Toolkit.RichTextBox richTextBok)
                {
                    bookUIs.Add(richTextBok.ToBookTextUI());
                }
            }

            return bookUIs;
        }
    }
}
