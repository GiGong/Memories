using Memories.Views;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace Memories.ViewModels
{
    public class MainWindowVM
    {
        #region Field

        private DelegateCommand _newBookCommand;
        private DelegateCommand _loadBookCommand;

        IDialogService _dialogService;

        #endregion

        #region Command

        public DelegateCommand NewBookCommand =>
            _newBookCommand ?? (_newBookCommand = new DelegateCommand(NewBook));

        public DelegateCommand LoadBookCommand =>
            _loadBookCommand ?? (_loadBookCommand = new DelegateCommand(LoadBook));

        #endregion Command

        public MainWindowVM(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Method

        private void NewBook()
        {

        }

        void LoadBook()
        {
            _dialogService.Show("EditBookView", null, null);
        }

        #endregion Method

    }
}
