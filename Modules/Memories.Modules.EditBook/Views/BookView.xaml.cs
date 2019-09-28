using Memories.Business.Models;
using Memories.Modules.EditBook.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace Memories.Modules.EditBook.Views
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : UserControl
    {
        public BookView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += BookView_PropertyChanged;
        }

        private void BookView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                var editBook = (Book)context.Value;
                (DataContext as BookViewVM).EditBook = editBook;
            }
        }
    }
}
