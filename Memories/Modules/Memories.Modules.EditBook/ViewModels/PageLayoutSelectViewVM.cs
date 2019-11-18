using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Converters;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Memories.Modules.EditBook.ViewModels
{
    public class PageLayoutSelectViewVM : DialogViewModelBase
    {
        #region Field

        private BookPageLayout _selectedItem;
        private ObservableCollection<BookPageLayout> _layouts;

        private DelegateCommand _checkCommand;

        private readonly IBookPageLayoutService _bookPageLayoutService;
        private readonly IFolderService _folderService;

        #endregion Field

        #region Property

        public BookPageLayout SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ObservableCollection<BookPageLayout> Layouts
        {
            get { return _layouts; }
            set { SetProperty(ref _layouts, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new DelegateCommand(ExecuteCheckCommand, CanExecuteCheckCommand).ObservesProperty(() => SelectedItem));

        #endregion Command

        #region Constructor

        public PageLayoutSelectViewVM(IBookPageLayoutService bookPageLayoutService, IFolderService folderService)
        {
            _bookPageLayoutService = bookPageLayoutService;
            _folderService = folderService;

            Title = (string)Application.Current.Resources["Program_Name"];
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            string path = _folderService.GetPageTemplateFolder(parameters.GetValue<PaperSize>("PaperSize").ToString());

            Layouts = new ObservableCollection<BookPageLayout>(_bookPageLayoutService.LoadPageLayoutsFromDirectory(path));

            BitmapSource image = new BitmapImage(new Uri("pack://application:,,,/Resources/Img/MemoriesWhite.jpg"));
            Layouts.Insert(0, new BookPageLayout
            {
                Name = "빈 페이지",
                PreviewSource = ByteArrayToImageSourceConverter.SourceToByteArray(image)
            });
        }

        private void ExecuteCheckCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK, new DialogParameters { { "PageLayout", SelectedItem } }));
        }

        private bool CanExecuteCheckCommand()
        {
            return SelectedItem != null;
        }

        #endregion Method
    }
}
