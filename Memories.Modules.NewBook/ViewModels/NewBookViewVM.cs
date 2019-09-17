using Memories.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace Memories.Modules.NewBook.ViewModels
{
    public class NewBookViewVM : DialogViewModelBase
    {
        internal const int NUM_OF_VIEWS = 2;

        #region Field

        private DelegateCommand _previousCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _checkCommand;

        #endregion Field

        #region Property

        /// <summary>
        /// Region Manager for New DialogWindow
        /// if don't use this, then RequestNavigate doesn't activate
        /// </summary>
        public IRegionManager RegionManager { get; set; }
        private NewBookNavigateParameter _parameter;
        public NewBookNavigateParameter Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(Previous, CanPrevious).ObservesProperty(()=>Parameter.NowPage));

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(Next, CanNext).ObservesProperty(()=>Parameter.NowPage));

        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        public DelegateCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new DelegateCommand(Check, CanCheck));

        #endregion Command

        #region Constructor

        public NewBookViewVM()
        {
            Parameter = new NewBookNavigateParameter() { NowPage = 0 };
            Parameter.IsCompleted.CollectionChanged += IsCompleted_CollectionChanged;
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            var param = new NavigationParameters();
            param.Add("Parameter", Parameter);
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, "InputBookInfoView", param);

            // this code load null in navigated viewmodel
            //RegionManager.RequestNavigate(RegionNames.NewBookRegion, "InputBookInfoView", new NavigationParameters($"NowPage={Parameter}"));
        }

        private void IsCompleted_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                CheckCommand.RaiseCanExecuteChanged();
            }
        }

        void Previous()
        {
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, "InputBookInfoView");
        }

        private bool CanPrevious()
        {
            return Parameter.NowPage > 0;
        }

        void Next()
        {
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, "LayoutSelectView");
        }

        private bool CanNext()
        {
            return Parameter.NowPage < NUM_OF_VIEWS - 1;
        }

        void Cancel()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        void Check()
        {
            // TODO Make Book with Information

            RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters()));
        }

        bool CanCheck()
        {
            foreach (var item in Parameter.IsCompleted)
            {
                if (item == false)
                {
                    return false;   
                }
            }
            return true;
        }

        #endregion Method
    }

    public class NewBookNavigateParameter : BindableBase
    {
        private int _nowPage;
        public int NowPage
        {
            get { return _nowPage; }
            set { SetProperty(ref _nowPage, value); }
        }

        private ObservableCollection<bool> _isCompleted = new ObservableCollection<bool>(Enumerable.Repeat(false, NewBookViewVM.NUM_OF_VIEWS));
        public ObservableCollection<bool> IsCompleted
        {
            get { return _isCompleted; }
            set { SetProperty(ref _isCompleted, value); }
        }
    }
}
