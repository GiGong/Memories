using Memories.Business.Models;
using Memories.Core;
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

        public EditBookViewVM(IBookService bookService, IFileService fileService, IApplicationCommands applicationCommands)
        {
            _bookService = bookService;
            _fileService = fileService;

            applicationCommands.NewBookCommand.RegisterCommand(NewBookCommand);
            applicationCommands.SaveCommand.RegisterCommand(SaveCommand);
            applicationCommands.SaveAsCommand.RegisterCommand(SaveAsCommand);
            applicationCommands.LoadCommand.RegisterCommand(LoadCommand);
            applicationCommands.CloseEditBookViewCommand.RegisterCommand(CloseEditBookViewCommand);
            
            Title = (string)Application.Current.Resources["Program_Name"];
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

            BookPath = parameters.GetValue<string>("BookPath");

            if (parameters.ContainsKey("NewBook"))
            {
                EditBook = parameters.GetValue<Book>("NewBook");
                _bookService.SaveBook(EditBook, BookPath);
            }
            else if (parameters.ContainsKey("LoadBook"))
            {
                EditBook = parameters.GetValue<Book>("LoadBook");
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

        void ExecuteNewBookCommand()
        {
            // Call NewBookView using Dialog Service Extension
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

        void ExecuteSaveCommand()
        {
            _bookService.SaveBook(EditBook, BookPath);
        }

        void ExecuteSaveAsCommand()
        {
            string path = _fileService.SaveFilePath();

            if (path != null)
            {
                BookPath = path;
                ExecuteSaveCommand();
            }
        }

        void ExecuteCloseEditBookViewCommand(DialogResult result)
        {
            RaiseRequestClose(result);
        }

        #endregion Method
    }
}
