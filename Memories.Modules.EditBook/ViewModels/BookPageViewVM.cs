using Memories.Core.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        private ObservableCollection<UIElement> _pageControls;
        public ObservableCollection<UIElement> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }

        private BookPage _nowPage;
        public BookPage NowPage
        {
            get { return _nowPage; }
            set { SetProperty(ref _nowPage, value); }
        }

        public BookPageViewVM()
        {

        }
    }
}
