using Memories.Business;
using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Core.Converters;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Memories.Core.Names;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        #region Field

        BookUI _newUI;

        private bool _isEditPage;
        private bool _isDraw;

        private ObservableCollection<UIElement> _pageControls;
        private byte[] _background;
        private BookPage _nowPage;

        private DelegateCommand _canvasCommand;
        private DelegateCommand _backgroundSelectCommand;
        private DelegateCommand _clearBackgroundCommand;
        private DelegateCommand<MMRichTextBox> _textBoxGotKeyboardFocusCommand;
        private DelegateCommand<MMCenterImage> _imageSelectCommand;
        private DelegateCommand<object> _drawControlCommand;
        private DelegateCommand<Rectangle> _drawEndCommand;
        private DelegateCommand<BookUI> _deleteUICommand;

        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #endregion Field

        #region Property

        public bool IsEditPage
        {
            get { return _isEditPage; }
            set { SetProperty(ref _isEditPage, value); }
        }

        public bool IsDraw
        {
            get { return _isDraw; }
            set { SetProperty(ref _isDraw, value); }
        }

        public ObservableCollection<UIElement> PageControls
        {
            get { return _pageControls; }
            set
            {
                SetProperty(ref _pageControls, value);
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

                NowPageChanged();
            }
        }

        #endregion Property

        #region Command

        public DelegateCommand CanvasCommand =>
            _canvasCommand ?? (_canvasCommand = new DelegateCommand(ExecuteCanvasCommand));

        public DelegateCommand BackgroundSelectCommand =>
            _backgroundSelectCommand ?? (_backgroundSelectCommand = new DelegateCommand(ExecuteBackgroundSelectCommand));

        public DelegateCommand ClearBackgroundCommand =>
            _clearBackgroundCommand ?? (_clearBackgroundCommand = new DelegateCommand(ExecuteClearBackgroundCommand));

        public DelegateCommand<MMRichTextBox> TextBoxGotKeyboardFocusCommand =>
            _textBoxGotKeyboardFocusCommand ?? (_textBoxGotKeyboardFocusCommand = new DelegateCommand<MMRichTextBox>(ExecuteTextBoxGotKeyboardFocusCommand));

        public DelegateCommand<MMCenterImage> ImageSelectCommand =>
            _imageSelectCommand ?? (_imageSelectCommand = new DelegateCommand<MMCenterImage>(ExecuteImageSelectCommand));

        public DelegateCommand<object> DrawControlCommand =>
            _drawControlCommand ?? (_drawControlCommand = new DelegateCommand<object>(ExecuteDrawControlCommand));

        public DelegateCommand<Rectangle> DrawEndCommand =>
            _drawEndCommand ?? (_drawEndCommand = new DelegateCommand<Rectangle>(ExecuteDrawEndCommand));

        public DelegateCommand<BookUI> DeleteUICommand =>
            _deleteUICommand ?? (_deleteUICommand = new DelegateCommand<BookUI>(ExecuteDeleteUICommand));

        #endregion Command

        #region Constructor

        public BookPageViewVM(IFileService fileService, IDialogService dialogService,
            IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            applicationCommands.DrawControlCommand.RegisterCommand(DrawControlCommand);

            _eventAggregator.GetEvent<DrawControlEndedEvent>().Subscribe(DrawAlreadyEnded);

            _newUI = null;
        }

        #endregion Constructor

        #region Method

        private void NowPageChanged()
        {
            _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Publish(null);

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

        private void ExecuteTextBoxGotKeyboardFocusCommand(MMRichTextBox richTextBox)
        {
            _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Publish(richTextBox);
        }

        private void ExecuteCanvasCommand()
        {
            _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Publish(null);
        }

        private void ExecuteDrawControlCommand(object parameter)
        {
            if (NowPage == null)
            {
                return;
            }
            _newUI = BookUI.GetBookUI((BookUIEnum)parameter);
            IsDraw = true;
        }

        private void ExecuteDrawEndCommand(Rectangle rect)
        {
            _newUI.GetPropertyFromRectangle(rect);
            NowPage.PageControls.Add(_newUI);
            PageControls = NowPage.ToUIElementCollection();
            _newUI = null;

            _eventAggregator.GetEvent<DrawControlEndedEvent>().Publish();
        }

        private void DrawAlreadyEnded()
        {
            IsDraw = false;
            _newUI = null;
        }

        private void ExecuteBackgroundSelectCommand()
        {
            SelectImage(null);
        }

        private void ExecuteClearBackgroundCommand()
        {
            Background = null;
        }

        private void ExecuteImageSelectCommand(MMCenterImage image)
        {
            SelectImage(image);
        }

        private void SelectImage(FrameworkElement element)
        {
            byte[] source;
            bool isImage = false;

            if (element is MMCenterImage image)
            {
                isImage = true;
                source = ByteArrayToImageSourceConverter.SourceToByteArray((BitmapSource)image.ImageSource);
            }
            else
            {
                source = Background;
            }

            _dialogService.ShowSelectImageDialog(new DialogParameters { { ParameterNames.SelectedImage, source } }, 
                (result) =>
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    byte[] selectedImgae = result.Parameters.GetValue<byte[]>(ParameterNames.SelectedImage);
                    if (isImage)
                    {
                        (element as MMCenterImage).ImageSource = (BitmapSource)new ImageSourceConverter().ConvertFrom(selectedImgae);
                    }
                    else
                    {
                        Background = selectedImgae;
                    }
                });

        }

        private void ExecuteDeleteUICommand(BookUI parameter)
        {
            if (MessageBox.Show("정말 이 컨트롤을 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                NowPage.PageControls.Remove(parameter);
                PageControls = NowPage.ToUIElementCollection();
            }
        }

        #endregion Method
    }
}
