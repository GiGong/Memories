using Memories.Models;
using Memories.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace Memories.Views
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
            var context = (ObservableObject<object>)sender;
            var nowPage = (BookPage)context.Value;
            (DataContext as BookPageViewVM).NowPage = nowPage;
        }
    }
}
