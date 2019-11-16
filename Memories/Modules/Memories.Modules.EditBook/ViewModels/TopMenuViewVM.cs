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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class TopMenuViewVM : BindableBase
    {
        #region Field

        private Book _editBook;

        private DelegateCommand _addPageCommand;
        private DelegateCommand _addManyPageCommand;
        private DelegateCommand _removePageCommand;
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

        public DelegateCommand AddManyPageCommand =>
            _addManyPageCommand ?? (_addManyPageCommand = new DelegateCommand(ExecuteAddManyPageCommand));

        public DelegateCommand RemovePageCommand =>
            _removePageCommand ?? (_removePageCommand = new DelegateCommand(ExecuteRemovePageCommand));

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
            AddPage(false);
        }

        private void ExecuteAddManyPageCommand()
        {
            AddPage(true);
        }

        private void AddPage(bool isMany)
        {
            var param = new DialogParameters
            {
                { "PaperSize", EditBook.PaperSize}
            };

            int pageCount = EditBook.BookPages.Count;
            int count = 1;
            var targetString = InputDialogWindow.Show("어디에 추가하시겠습니까?" + Environment.NewLine +
                                    "범위 : " + 1 + " ~ " + pageCount, "페이지 추가하기", MessageBoxImage.Question, true);

            if (int.TryParse(targetString, out int target) && (0 < target && target < pageCount + 1))
            {
                if (isMany)
                {
                    var countString = InputDialogWindow.Show("몇 장 추가하시겠습니까??", "페이지 추가하기", MessageBoxImage.Question, true);
                    if (!int.TryParse(countString, out count) || count < 1)
                    {
                        MessageBox.Show("잘못된 입력입니다!", "Memories", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                _dialogService.ShowDialog(nameof(PageLayoutSelectView), param,
                (result) =>
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    var layout = result.Parameters.GetValue<BookPageLayout>("PageLayout");

                    for (int i = 0; i < count; i++)
                    {
                        EditBook.BookPages.Insert(target - 1 /* using for ordered inserting + i*/, layout.Page);
                    }
                });
            }
        }

        private void ExecuteRemovePageCommand()
        {
            int count = EditBook.BookPages.Count;
            if (count < 1)
            {
                return;
            }

            var result = InputDialogWindow.Show("몇번 페이지를 지우시겠습니까?" + Environment.NewLine +
                                                "범위 : " + 1 + " ~ " + count, "페이지 지우기", MessageBoxImage.Question, true);
            if (int.TryParse(result, out int num) && (0 < num && num < count + 1))
            {
                if (MessageBox.Show("정말 " + num + " 페이지를 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                {
                    EditBook.BookPages.RemoveAt(num - 1);
                }
            }
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
