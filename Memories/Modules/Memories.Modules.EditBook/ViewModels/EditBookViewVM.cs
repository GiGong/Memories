using Memories.Business;
using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Modules.EditBook.Enums;
using Memories.Modules.EditBook.Parameters;
using Memories.Modules.EditBook.Views;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class EditBookViewVM : DialogViewModelBase
    {
        #region Field

        private Book _editBook;
        private string _bookPath;

        private ObservableCollection<ExportParameter> _exportMenus;
        private ObservableCollection<MenuParameter> _extensionMenus;
        private ObservableCollection<DrawParameter> _drawControls;

        private DelegateCommand _loadCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _addPageCommand;

        private DelegateCommand<MenuParameter> _menuCommand;
        private DelegateCommand<ExportParameter> _exportCommand;

        private readonly IBookService _bookService;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IExportToImageService _exportToImageService;

        private IApplicationCommands _applicationCommands;

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

        public ObservableCollection<MenuParameter> ExtensionMenus
        {
            get { return _extensionMenus; }
            set { SetProperty(ref _extensionMenus, value); }
        }

        public ObservableCollection<DrawParameter> DrawControls
        {
            get { return _drawControls; }
            set { SetProperty(ref _drawControls, value); }
        }

        public ObservableCollection<ExportParameter> ExportMenus
        {
            get { return _exportMenus; }
            set { SetProperty(ref _exportMenus, value); }
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(ExecuteLoadCommand));

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        public DelegateCommand AddPageCommand =>
            _addPageCommand ?? (_addPageCommand = new DelegateCommand(ExecuteAddPageCommand));

        public DelegateCommand<MenuParameter> MenuCommand =>
            _menuCommand ?? (_menuCommand = new DelegateCommand<MenuParameter>(ExecuteMenuCommand));

        public DelegateCommand<ExportParameter> ExportCommand =>
            _exportCommand ?? (_exportCommand = new DelegateCommand<ExportParameter>(ExecuteExportCommand));

        #endregion Command

        #region Constructor

        public EditBookViewVM(IBookService bookService, IFileService fileService, IDialogService dialogService,
                                IExportToImageService exportToImageService, IApplicationCommands applicationCommands)
        {
            _bookService = bookService;

            _fileService = fileService;
            _dialogService = dialogService;
            _exportToImageService = exportToImageService;

            ApplicationCommands = applicationCommands;

            Title = (string)Application.Current.Resources["Program_Name"];

            ExtensionMenus = new ObservableCollection<MenuParameter>
            {
                new MenuParameter("새 책 만들기", MenuType.New),
                new MenuParameter("책 열기", MenuType.Load),
                new MenuParameter("저장하기", MenuType.Save),
                new MenuParameter("다른이름으로 저장하기", MenuType.SaveAs),
                new MenuParameter("처음 화면으로 돌아가기", MenuType.BackToStartWindow),
                new MenuParameter("닫기", MenuType.Close),
            };

            ExportMenus = new ObservableCollection<ExportParameter>
            {
                new ExportParameter("이미지로 내보내기", ExportType.Image ),
                new ExportParameter("PDF로 내보내기", ExportType.PDF ),
                new ExportParameter("출력하기", ExportType.Print)
            };

            DrawControls = new ObservableCollection<DrawParameter>
            {
                new DrawParameter("사진 추가하기", BookUIEnum.ImageUI),
                new DrawParameter("글 추가하기", BookUIEnum.TextUI)
            };
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

        #region Command Method

        void ExecuteLoadCommand()
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

        void ExecuteMenuCommand(MenuParameter parameter)
        {
            switch (parameter.Type)
            {
                case MenuType.New:
                    break;
                case MenuType.Load:
                    ExecuteLoadCommand();
                    break;
                case MenuType.Save:
                    ExecuteSaveCommand();
                    break;
                case MenuType.SaveAs:
                    break;

                case MenuType.Close:
                    RaiseRequestClose(new DialogResult(ButtonResult.None));
                    break;
                case MenuType.BackToStartWindow:
                    RaiseRequestClose(new DialogResult(ButtonResult.Retry));
                    break;
            }
        }

        void ExecuteExportCommand(ExportParameter parameter)
        {
            switch (parameter.Type)
            {
                case ExportType.Image:
                    ExportToImage();
                    break;
                case ExportType.PDF:
                    ExportToPdf();
                    break;
                case ExportType.Print:
                    ExportToPrint();
                    break;
            }
        }

        #endregion Command Method

        private void PageLayoutSelected(IDialogResult result)
        {
            if (result.Result != ButtonResult.OK)
            {
                return;
            }

            var layout = result.Parameters.GetValue<BookPageLayout>("PageLayout");
            EditBook.BookPages.Add(layout.Page);
        }

        private void ExportToImage()
        {
            //MMMessageBox.Show("폴더를 새로 생성하고, 그 안에 Image파일들을 넣습니다.");

            string path = _fileService.SaveFilePath(
                string.Join("|", ExtentionFilters.JPEG, ExtentionFilters.PNG, ExtentionFilters.BMP, ExtentionFilters.ImageFiles));

            if (path == null)
            {
                return;
            }

            ImageFormat format = ImageFormat.JPEG;

            switch (Path.GetExtension(path))
            {
                case ".png":
                    format = ImageFormat.PNG;
                    break;

                case ".bmp":
                    format = ImageFormat.BMP;
                    break;
            }

            _exportToImageService.ExportBookToImage(EditBook, format, path);
        }

        private void ExportToPdf()
        {

        }

        private void ExportToPrint()
        {

        }



        #endregion Method
    }
}
