using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Extensions;
using Memories.Core.Names;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class EditBookViewVM : DialogViewModelBase
    {
        #region Field

        private Book _editBook;
        private string _bookPath;

        private DelegateCommand _newBookCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _loadCommand;
        private DelegateCommand _saveAsCommand;
        private DelegateCommand<DialogResult> _closeEditBookViewCommand;

        private readonly IBookService _bookService;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;

        #endregion Field

        #region Property

        public Book EditBook
        {
            get { return _editBook; }
            set { SetProperty(ref _editBook, value); }
        }

        public string BookPath
        {
            get { return _bookPath; }
            set { SetProperty(ref _bookPath, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand NewBookCommand =>
            _newBookCommand ?? (_newBookCommand = new DelegateCommand(ExecuteNewBookCommand));

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand SaveAsCommand =>
            _saveAsCommand ?? (_saveAsCommand = new DelegateCommand(ExecuteSaveAsCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(ExecuteLoadCommand));

        public DelegateCommand<DialogResult> CloseEditBookViewCommand =>
            _closeEditBookViewCommand ?? (_closeEditBookViewCommand = new DelegateCommand<DialogResult>(ExecuteCloseEditBookViewCommand));


        #endregion Command

        #region Constructor

        public EditBookViewVM(IBookService bookService, IFileService fileService, IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _bookService = bookService;
            _fileService = fileService;
            _dialogService = dialogService;

            applicationCommands.NewBookCommand.RegisterCommand(NewBookCommand);
            applicationCommands.SaveCommand.RegisterCommand(SaveCommand);
            applicationCommands.SaveAsCommand.RegisterCommand(SaveAsCommand);
            applicationCommands.LoadCommand.RegisterCommand(LoadCommand);
            applicationCommands.CloseEditBookViewCommand.RegisterCommand(CloseEditBookViewCommand);

            Title = (string)Application.Current.Resources["Designed_Program_Name"];
        }

        #endregion Constructor

        #region Opened

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            if (parameters == null)
            {
                return;
            }

            BookPath = parameters.GetValue<string>(ParameterNames.BookPath);

            if (parameters.ContainsKey(ParameterNames.NewBook))
            {
                EditBook = parameters.GetValue<Book>(ParameterNames.NewBook);
                ExecuteSaveCommand();
            }
            else if (parameters.ContainsKey(ParameterNames.LoadBook))
            {
                EditBook = parameters.GetValue<Book>(ParameterNames.LoadBook);
            }
        }

        #endregion

        #region Closing

        public override bool CanCloseDialog()
        {
            //TODO: Check edited content. save or ignore
            return base.CanCloseDialog();
        }

        #endregion Closing

        #region Method

        private bool CanBookCommand()
        {
            return EditBook != null;
        }

        private void ExecuteNewBookCommand()
        {
            var messageBoxResult = MessageBox.Show("현재 책을 저장하시겠습니까?", "Memories", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ExecuteSaveCommand();
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                _dialogService.ShowNewBookDialog(null,
                    (result) =>
                    {
                        if (result.Result == ButtonResult.OK)
                        {
                            //TODO: Check "save current book"
                            BookPath = result.Parameters.GetValue<string>(ParameterNames.BookPath);
                            EditBook = result.Parameters.GetValue<Book>(ParameterNames.NewBook);
                            ExecuteSaveCommand();
                        }
                    });
            }
        }

        void ExecuteLoadCommand()
        {
            string path = _fileService.OpenFilePath();

            if (path != null)
            {
                BookPath = path;
                EditBook = _bookService.LoadBook(BookPath);
            }
        }

        private void ExecuteSaveCommand()
        {
            _bookService.SaveBook(EditBook, BookPath);
        }

        private void ExecuteSaveAsCommand()
        {
            string path = _fileService.SaveFilePath();

            if (path != null)
            {
                BookPath = path;
                ExecuteSaveCommand();
            }
        }

        private void ExecuteCloseEditBookViewCommand(DialogResult result)
        {
            RaiseRequestClose(result);
        }

        #endregion Method
    }
}
