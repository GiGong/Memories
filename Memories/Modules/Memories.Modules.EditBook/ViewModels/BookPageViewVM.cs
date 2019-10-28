using Memories.Business;
using Memories.Business.Models;
using Memories.Core.Controls;
using Memories.Core.Converters;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
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

        private DelegateCommand<FrameworkElement> _canvasClickCommand;
        private DelegateCommand<MMCenterImage> _imageSelectCommand;

        private readonly IFileService _fileService;
        private readonly IEventAggregator _eventAggregator;

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
            set
            {
                SetProperty(ref _pageControls, value);

                if (PageControls != null)
                {
                    foreach (var element in PageControls)
                    {
                        if (element is MMRichTextBox richTextBox)
                        {
                            richTextBox.GotFocus += (s, e) => _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Publish(s as MMRichTextBox);
                        }
                    }
                }
            }
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

        public BookPageViewVM(IFileService fileService, IEventAggregator eventAggregator)
        {
            _fileService = fileService;
            _eventAggregator = eventAggregator;
        }

        #endregion Constructor

        #region Command

        public DelegateCommand<MMCenterImage> ImageSelectCommand =>
            _imageSelectCommand ?? (_imageSelectCommand = new DelegateCommand<MMCenterImage>(ExecuteSelectImageCommand));

        public DelegateCommand<FrameworkElement> CanvasClickCommand =>
            _canvasClickCommand ?? (_canvasClickCommand = new DelegateCommand<FrameworkElement>(ExecuteCanvasClickCommand));

        #endregion Command

        #region Method

        private void ExecuteCanvasClickCommand(FrameworkElement element)
        {
            if (element is MMCenterImage || element is Canvas)
            {
                ExecuteSelectImageCommand(element);
            }
        }

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
