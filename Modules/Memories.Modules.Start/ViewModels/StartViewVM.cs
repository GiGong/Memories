using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Memories.Modules.Start.ViewModels
{
    public class StartViewVM : BindableBase
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

        public StartViewVM(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Method

        private void NewBook()
        {
            _dialogService.ShowDialog("NewBookView", null, (result) =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    _dialogService.Show("EditBookView", result.Parameters, null);
                }
            });
        }

        void LoadBook()
        {
            _dialogService.Show("EditBookView", null, null);
        }

        #endregion Method
    }
}
