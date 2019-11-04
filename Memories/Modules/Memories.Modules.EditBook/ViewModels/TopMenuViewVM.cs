using Memories.Business;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Core.Extensions;
using Memories.Modules.EditBook.Views;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class TopMenuViewVM : BindableBase
    {
        #region Field

        private Book _editBook;

        private DelegateCommand _addPageCommand;
        private DelegateCommand _deletePageCommand;
        private DelegateCommand _exportToImageCommand;
        private DelegateCommand _exportToPDFCommand;
        private DelegateCommand _printCommand;

        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IExportToImageService _exportToImageService;
        private readonly IPrintService _printService;
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

        public DelegateCommand DeletePageCommand =>
            _deletePageCommand ?? (_deletePageCommand = new DelegateCommand(ExecuteDeletePageCommand));

        public DelegateCommand ExportToImageCommand =>
            _exportToImageCommand ?? (_exportToImageCommand = new DelegateCommand(ExecuteExportToImageCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand ExportToPDFCommand =>
            _exportToPDFCommand ?? (_exportToPDFCommand = new DelegateCommand(ExecuteExportToPDFCommand, CanBookCommand).ObservesProperty(() => EditBook));

        public DelegateCommand PrintCommand =>
            _printCommand ?? (_printCommand = new DelegateCommand(ExecutePrintCommand, CanBookCommand).ObservesProperty(() => EditBook));

        #endregion Command

        #region Constructor

        public TopMenuViewVM(IFileService fileService, IDialogService dialogService, IExportToImageService exportToImageService,
                                IPrintService printService, IApplicationCommands applicationCommands)
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _exportToImageService = exportToImageService;
            _printService = printService;

            ApplicationCommands = applicationCommands;
        }

        #endregion Constructor

        #region Method

        private bool CanBookCommand()
        {
            return EditBook != null;
        }

        private void ExecuteAddPageCommand()
        {
            var param = new DialogParameters
            {
                { "PaperSize", EditBook.PaperSize}
            };

            _dialogService.ShowDialog(nameof(PageLayoutSelectView), param, PageLayoutSelected);
        }

        private void ExecuteDeletePageCommand()
        {
            int count = EditBook.BookPages.Count;
            if (count < 1)
            {
                return;
            }

            var result = InputDialogWindow.Show("몇번 페이지를 지우시겠습니까?\n"+
                                                1 + " ~ " + count, "페이지 지우기", MessageBoxImage.Question, true);
            if (int.TryParse(result, out int num))
            {
                int index = num - 1;
                if (-1 < index && index < count)
                {
                    if (MessageBox.Show("정말 " + num + " 페이지를 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                        == MessageBoxResult.Yes)
                    {
                        EditBook.BookPages.RemoveAt(index);
                    }
                }
            }

            //TODO: Composite Command로 BookView에 보내서 왼쪽 오른쪽 중 삭제할 수 있게
            //var result = MessageBox.Show("왼쪽 페이지를 지우시려면 [예]\n오른쪽 페이지를 지우시려면 [아니오]\n지우지 않으려면 [취소]",
            //    "페이지 삭제", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            //switch (result)
            //{
            //    case MessageBoxResult.Yes:
            //        break;
            //    case MessageBoxResult.No:
            //        break;
            //    default:
            //        break;
            //}
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
            _printService.Print(EditBook.ToFixedDocument().DocumentPaginator);
        }

        #endregion Method
    }
}
