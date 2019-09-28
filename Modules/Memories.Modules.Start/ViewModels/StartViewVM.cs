using Memories.Core;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.IO;
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
        private readonly IFileService _fileService;
        private readonly IBookService _bookService;

        #endregion

        #region Command

        public DelegateCommand NewBookCommand =>
            _newBookCommand ?? (_newBookCommand = new DelegateCommand(NewBook));

        public DelegateCommand LoadBookCommand =>
            _loadBookCommand ?? (_loadBookCommand = new DelegateCommand(LoadBook));

        #endregion Command

        public StartViewVM(IDialogService dialogService, IFileService fileService, IBookService bookService, IApplicationCommands applicationCommands)
        {
            _dialogService = dialogService;
            _fileService = fileService;
            _bookService = bookService;
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

                        _dialogService.Show("EditBookView", result.Parameters, EditBookView_Closed);
                    }
                });
        }

        private void LoadBook()
        {
            string path = _fileService.OpenFilePath();
            if (path != null && File.Exists(path))
            {
                var param = new DialogParameters();
                param.Add("LoadBook", _bookService.LoadBook(path));
                _dialogService.Show("EditBookView", param, EditBookView_Closed);
            }
        }

        private void EditBookView_Closed(IDialogResult result)
        {
            if (result.Result == ButtonResult.None)
            {
                Application.Current.Shutdown();
            }
            else if (result.Result == ButtonResult.Retry)
            {
                _applicationCommands.ShowShellCommand.Execute("");
            }
        }

        #endregion Method
    }
}
