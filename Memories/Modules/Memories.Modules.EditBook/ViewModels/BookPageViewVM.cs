using Memories.Business;
using Memories.Business.Models;
using Memories.Core.Controls;
using Memories.Core.Converters;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        #region Field

        private bool _isEditPage;

        private ObservableCollection<UIElement> _pageControls;
        private byte[] _background;
        private BookPage _nowPage;

        private DelegateCommand<MMCenterImage> _imageClickCommand;
        private DelegateCommand<Canvas> _canvasClickCommand;

        private readonly IFileService _fileService;

        #endregion Field

        #region Property

        public bool IsEditPage
        {
            get { return _isEditPage; }
            set { SetProperty(ref _isEditPage, value); }
        }

        public ObservableCollection<UIElement> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }

        public byte[] Background
        {
            get { return _background; }
            set
            {
                SetProperty(ref _background, value);

                if (NowPage != null)
                {
                    NowPage.Background = Background;
                }
            }
        }

        public BookPage NowPage
        {
            get { return _nowPage; }
            set
            {
                SetProperty(ref _nowPage, value);

                if (NowPage == null)
                {
                    IsEditPage = false;
                    Background = null;
                    PageControls = null;
                }
                else
                {
                    IsEditPage = true;
                    Background = NowPage.Background;
                    PageControls = NowPage.ToUIElementCollection();
                }
            }
        }

        #endregion Property

        #region Constructor

        public BookPageViewVM(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion Constructor

        #region Command

        public DelegateCommand<MMCenterImage> ImageClickCommand =>
            _imageClickCommand ?? (_imageClickCommand = new DelegateCommand<MMCenterImage>(ExecuteSelectImageCommand));

        public DelegateCommand<Canvas> CanvasClickCommand =>
            _canvasClickCommand ?? (_canvasClickCommand = new DelegateCommand<Canvas>(ExecuteSelectImageCommand));

        #endregion Command

        #region Method

        private void ExecuteSelectImageCommand(FrameworkElement frameworkElement)
        {
            if (!(frameworkElement is MMCenterImage || frameworkElement is Canvas))
            {
                throw new ArgumentException(frameworkElement + " is not image or canvas.");
            }

            string filter = ExtentionFilters.ImageFiles;
            string path = _fileService.OpenFilePath(filter);

            if (path == null)
            {
                return;
            }

            var bitmap = new BitmapImage(new Uri(path, UriKind.Relative));
            try
            {
                if (frameworkElement is MMCenterImage image)
                {
                    image.ImageSource = bitmap;
                }
                else if (frameworkElement is Canvas canvas)
                {
                    Background = ByteArrayToImageSourceConverter.SourceToByteArray(bitmap);
                }
            }
            catch (NotSupportedException)
            {
                MMMessageBox.Show("Not supported file" + Environment.NewLine + "지원하지 않는 파일입니다.", "Memories", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MMMessageBox.Show(e.Message);
            }
        }

        #endregion Method
    }
}
