using Memories.Business.Models;
using Memories.Modules.EditBook.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace Memories.Modules.EditBook.Views
{
    /// <summary>
    /// Interaction logic for BookPageView.xaml
    /// </summary>
    public partial class BookPageView : UserControl
    {
        public BookPageView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += BookPageView_PropertyChanged;
        }

        private void BookPageView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                var nowPage = (BookPage)context.Value;
                (DataContext as BookPageViewVM).NowPage = nowPage;
            }
        }
    }
}
