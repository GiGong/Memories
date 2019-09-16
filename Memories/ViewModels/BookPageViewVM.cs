using Memories.Models;
using Prism.Commands;
using Prism.Mvvm;
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

namespace Memories.ViewModels
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
