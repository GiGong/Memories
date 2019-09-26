using Memories.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.Start.ViewModels
{
    public class StartViewVM : BindableBase
    {
        #region Field

        private DelegateCommand _newBookCommand;
        private DelegateCommand _loadBookCommand;

        private IDialogService _dialogService;
        private readonly IApplicationCommands _applicationCommands;

        #endregion

        #region Command

        public DelegateCommand NewBookCommand =>
            _newBookCommand ?? (_newBookCommand = new DelegateCommand(NewBook));

        public DelegateCommand LoadBookCommand =>
            _loadBookCommand ?? (_loadBookCommand = new DelegateCommand(LoadBook));

        #endregion Command

        public StartViewVM(IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
        }

        #region Method

        private void NewBook()
        {
            _dialogService.ShowDialog("NewBookView", null,
                (result) =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        _applicationCommands.HideShellCommand.Execute("");

                        _dialogService.Show("EditBookView", result.Parameters,
                            (resultEdit) =>
                            {
                                if (resultEdit.Result == ButtonResult.None)
                                {
                                    Application.Current.Shutdown();
                                }
                                else if (resultEdit.Result == ButtonResult.Retry)
                                {
                                    _applicationCommands.ShowShellCommand.Execute("");
                                }
                            });
                    }
                });
        }

        void LoadBook()
        {
            //TODO: Select book in open file dialog, Show in EditBookView
            _dialogService.Show("EditBookView", null, (result) =>
            {
                MessageBox.Show(result.Result.ToString());
            });
        }

        #endregion Method
    }
}
