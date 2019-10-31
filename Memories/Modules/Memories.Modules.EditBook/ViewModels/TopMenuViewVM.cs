using Memories.Business;
using Memories.Business.Models;
using Memories.Core;
using Memories.Modules.EditBook.Views;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Memories.Modules.EditBook.ViewModels
{
    public class TopMenuViewVM : BindableBase
    {
        #region Field
        
        private Book _editBook;

        private DelegateCommand _addPageCommand;
        private DelegateCommand _exportToImageCommand;
        private DelegateCommand _exportToPDFCommand;
        private DelegateCommand _printCommand;

        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IExportToImageService _exportToImageService;

        private IApplicationCommands _applicationCommands;

        #endregion Field

        #region Property

        public DialogResult BackToStartWindowResult { get; } = new DialogResult(ButtonResult.Retry);
        public DialogResult CloseResult { get; } = new DialogResult(ButtonResult.None);


        public Book EditBook
        {
            get { return _editBook; }
            set { SetProperty(ref _editBook, value); }
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand AddPageCommand =>
            _addPageCommand ?? (_addPageCommand = new DelegateCommand(ExecuteAddPageCommand));

        public DelegateCommand ExportToImageCommand =>
            _exportToImageCommand ?? (_exportToImageCommand = new DelegateCommand(ExecuteExportToImageCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand ExportToPDFCommand =>
            _exportToPDFCommand ?? (_exportToPDFCommand = new DelegateCommand(ExecuteExportToPDFCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand PrintCommand =>
            _printCommand ?? (_printCommand = new DelegateCommand(ExecutePrintCommand, CanBookCommand).ObservesProperty(() => EditBook));

        #endregion Command

        #region Constructor

        public TopMenuViewVM(IFileService fileService, IDialogService dialogService, IExportToImageService exportToImageService, IApplicationCommands applicationCommands)
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _exportToImageService = exportToImageService;

            ApplicationCommands = applicationCommands;
        }

        #endregion Constructor

        #region Method

        private bool CanBookCommand()
        {
            return EditBook != null;
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

        private void ExecuteExportToImageCommand()
        {
            //MMMessageBox.Show("폴더를 새로 생성하고, 그 안에 Image파일들을 넣습니다.");

            string path = _fileService.SaveFilePath(
                string.Join("|", ExtentionFilters.JPEG, ExtentionFilters.PNG, ExtentionFilters.BMP, ExtentionFilters.ImageFiles));

            if (path == null)
            {
                return;
            }

            _exportToImageService.ExportBookToImage(EditBook, path);
        }

        private void ExecuteExportToPDFCommand()
        {

        }

        private void ExecutePrintCommand()
        {

        }

        #endregion Method
    }
}
