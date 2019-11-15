using Memories.Business.Models;
using Memories.Modules.NewBook.ViewModels;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Memories.Modules.NewBook
{
    public class NewBookNavigationParameter : BindableBase
    {
        private string _controlState;
        private int _nowPage;
        private ObservableCollection<bool> _isCompleted = new ObservableCollection<bool>(Enumerable.Repeat(false, NewBookViewVM.NUM_OF_VIEWS));

        public Book InputBook { get; set; }
        public string BookPath { get; set; }


        public string ControlState
        {
            get { return _controlState; }
            set { SetProperty(ref _controlState, value); }
        }

        public int NowPage
        {
            get { return _nowPage; }
            set { SetProperty(ref _nowPage, value); }
        }

        public ObservableCollection<bool> IsCompleted
        {
            get { return _isCompleted; }
            set { SetProperty(ref _isCompleted, value); }
        }
    }
}
