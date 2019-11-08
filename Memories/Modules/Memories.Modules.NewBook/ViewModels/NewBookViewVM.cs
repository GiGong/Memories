using Memories.Core;
using Memories.Core.Names;
using Memories.Modules.NewBook.Views;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.NewBook.ViewModels
{
    public class NewBookViewVM : DialogViewModelBase
    {
        internal const int NUM_OF_VIEWS = 2;

        #region Field

        private readonly IBookService _bookService;

        private DelegateCommand _previousCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _checkCommand;

        private NewBookNavigationParameter _parameter;

        #endregion Field

        #region Property

        /// <summary>
        /// Region Manager for New DialogWindow
        /// if don't use this, then RequestNavigate doesn't activate
        /// </summary>
        public IRegionManager RegionManager { get; set; }
        public NewBookNavigationParameter Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(Previous, CanPrevious).ObservesProperty(() => Parameter.NowPage));

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(Next, CanNext).ObservesProperty(() => Parameter.NowPage));

        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        public DelegateCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new DelegateCommand(Check, CanCheck));

        #endregion Command

        #region Constructor

        public NewBookViewVM(IBookService bookService)
        {
            Title = (string)Application.Current.Resources["Program_Name"];

            _bookService = bookService;

            Parameter = new NewBookNavigationParameter() { NowPage = 0, InputBook = _bookService.GetEmptyBook() };
            Parameter.IsCompleted.CollectionChanged += IsCompleted_CollectionChanged;
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            var param = new NavigationParameters
            {
                { "Parameter", Parameter }
            };
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, nameof(InputBookInfoView), param);

            // this code loads null in navigated viewmodel
            //RegionManager.RequestNavigate(RegionNames.NewBookRegion, "InputBookInfoView", new NavigationParameters($"NowPage={Parameter}"));
        }

        private void IsCompleted_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                CheckCommand.RaiseCanExecuteChanged();
            }
        }

        private void Previous()
        {
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, nameof(InputBookInfoView));
        }

        private bool CanPrevious()
        {
            return Parameter.NowPage > 0;
        }

        private void Next()
        {
            RegionManager.RequestNavigate(RegionNames.NewBookRegion, nameof(BookLayoutSelectView));
        }

        private bool CanNext()
        {
            return Parameter.NowPage < NUM_OF_VIEWS - 1;
        }

        private void Cancel()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private void Check()
        {
            var param = new DialogParameters
            {
                { ParameterNames.NewBook, Parameter.InputBook.Clone() },
                { ParameterNames.BookPath, Parameter.BookPath }
            };

            RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }

        private bool CanCheck()
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
}
