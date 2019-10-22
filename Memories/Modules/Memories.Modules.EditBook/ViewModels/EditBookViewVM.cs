using Memories.Business;
using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Modules.EditBook.Views;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.IO;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class EditBookViewVM : DialogViewModelBase
    {
        #region Field

        private Book _editBook;
        private string _bookPath;

        private DelegateCommand _openCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _addPageCommand;
        private DelegateCommand _exportToImageCommand;
        private DelegateCommand _openStartWindowCommand;

        private readonly IBookService _bookService;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IExportToImageService _exportToImageService;

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

        public DelegateCommand OpenCommand =>
            _openCommand ?? (_openCommand = new DelegateCommand(ExecuteOpenCommand));

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        public DelegateCommand OpenStartWindowCommand =>
            _openStartWindowCommand ?? (_openStartWindowCommand = new DelegateCommand(ExecuteOpenStartWindowCommand));

        public DelegateCommand AddPageCommand =>
            _addPageCommand ?? (_addPageCommand = new DelegateCommand(ExecuteAddPageCommand));

        public DelegateCommand ExportToImageCommand =>
            _exportToImageCommand ?? (_exportToImageCommand = new DelegateCommand(ExecuteExportToImageCommand));

        #endregion Command

        #region Constructor

        public EditBookViewVM(IBookService bookService, IFileService fileService, IDialogService dialogService,
                                IExportToImageService exportToImageService)
        {
            _bookService = bookService;

            _fileService = fileService;
            _dialogService = dialogService;
            _exportToImageService = exportToImageService;

            Title = (string)Application.Current.Resources["Program_Name"];
        }

        #endregion Constructor

        #region Method

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
                ExecuteSaveCommand();
            }
            else if (parameters.ContainsKey("LoadBook"))
            {
                EditBook = parameters.GetValue<Book>("LoadBook");
            }
        }

        void ExecuteOpenStartWindowCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Retry));
        }

        void ExecuteOpenCommand()
        {
            BookPath = _fileService.OpenFilePath();
            if (BookPath == null)
            {
                return;
            }

            EditBook = _bookService.LoadBook(BookPath);
        }

        void ExecuteSaveCommand()
        {
            if (EditBook != null)
            {
                _bookService.SaveBook(EditBook, BookPath);
            }
        }

        void ExecuteAddPageCommand()
        {
            var param = new DialogParameters
            {
                { "PaperSize", EditBook.PaperSize}
            };

            _dialogService.ShowDialog(nameof(PageLayoutSelectView), param, PageLayoutSelected);
        }

        private void PageLayoutSelected(IDialogResult result)
        {
            if (result.Result != ButtonResult.OK)
            {
                return;
            }

            var layout = result.Parameters.GetValue<BookPageLayout>("PageLayout");
            EditBook.BookPages.Add(layout.Page);
        }

        void ExecuteExportToImageCommand()
        {
            //MMMessageBox.Show("폴더를 새로 생성하고, 그 안에 Image파일들을 넣습니다.");

            string path = _fileService.SaveFilePath(
                string.Join("|",
                ExtentionFilters.JPEG,
                ExtentionFilters.PNG,
                ExtentionFilters.BMP,
                ExtentionFilters.ImageFiles));

            ImageFormat format = ImageFormat.JPEG;

            switch (Path.GetExtension(path))
            {
                case ".jpg":
                case ".jpeg":
                default:
                    break;

                case ".png":
                    format = ImageFormat.PNG;
                    break;

                case ".bmp":
                    format = ImageFormat.BMP;
                    break;
            }

            _exportToImageService.ExportBookToImage(EditBook, format, path);
        }

        #endregion Method
    }
}
