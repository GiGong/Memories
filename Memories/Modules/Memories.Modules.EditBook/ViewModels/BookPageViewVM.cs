using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Core.Converters;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Memories.Core.Names;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        #region Field

        private BookUI _newUI;

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
        private DelegateCommand<object[]> _imageDropCommand;
        private DelegateCommand<BookImageUI> _imageRemoveCommand;
        private DelegateCommand<object> _drawControlCommand;
        private DelegateCommand _drawCancelCommand;
        private DelegateCommand<Rectangle> _drawEndCommand;
        private DelegateCommand<BookUI> _deleteUICommand;

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

        public DelegateCommand<object[]> ImageDropCommand =>
            _imageDropCommand ?? (_imageDropCommand = new DelegateCommand<object[]>(ExecuteImageDropCommand, CanExecuteImageDropCommand));

        public DelegateCommand<BookImageUI> ImageRemoveCommand =>
            _imageRemoveCommand ?? (_imageRemoveCommand = new DelegateCommand<BookImageUI>(ExecuteImageRemoveCommand));

        public DelegateCommand<object> DrawControlCommand =>
            _drawControlCommand ?? (_drawControlCommand = new DelegateCommand<object>(ExecuteDrawControlCommand));

        public DelegateCommand DrawCancelCommand =>
            _drawCancelCommand ?? (_drawCancelCommand = new DelegateCommand(ExecuteDrawCancelCommand, CanExecuteDrawCancelCommand).ObservesProperty(() => IsDraw));

        public DelegateCommand<Rectangle> DrawEndCommand =>
            _drawEndCommand ?? (_drawEndCommand = new DelegateCommand<Rectangle>(ExecuteDrawEndCommand));

        public DelegateCommand<BookUI> DeleteUICommand =>
            _deleteUICommand ?? (_deleteUICommand = new DelegateCommand<BookUI>(ExecuteDeleteUICommand));

        #endregion Command

        #region Constructor

        public BookPageViewVM(IDialogService dialogService,
            IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            applicationCommands.DrawControlCommand.RegisterCommand(DrawControlCommand);
            applicationCommands.DrawCancelCommand.RegisterCommand(DrawCancelCommand);

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
                IsDraw = false;
                _newUI = null;
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

        private bool CanExecuteDrawCancelCommand()
        {
            return IsDraw || NowPage == null;
        }

        private void ExecuteDrawCancelCommand()
        {
            _newUI = null;
            IsDraw = false;
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
            if (MessageBox.Show("정말 배경사진을 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
            {
                Background = null;
            }
        }

        private void ExecuteImageSelectCommand(MMCenterImage image)
        {
            SelectImage(image);
        }

        private bool CanExecuteImageDropCommand(object[] parameters)
        {
            if (parameters != null 
                && parameters.Length == 2
                && parameters[0] is MMCenterImage
                && parameters[1] is string)
            {
                return true;
            }
            return false;
        }

        private void ExecuteImageDropCommand(object[] parameters)
        {
            try
            {
                MMCenterImage image = (MMCenterImage)parameters[0];
                string path = (string)parameters[1];
                image.ImageSource = new BitmapImage(new Uri(path));
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("지원되지 않는 파일입니다.", "Memories", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteImageRemoveCommand(BookImageUI imageUI)
        {
            if (imageUI == null || imageUI.ImageSource == null)
            {
                return;
            }
            if (MessageBox.Show("정말 사진을 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
            {
                imageUI.ImageSource = null;
            }
        }

        private void ExecuteDeleteUICommand(BookUI parameter)
        {
            if (MessageBox.Show("정말 이 영역을 지우시겠습니까?", "Memories", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                NowPage.PageControls.Remove(parameter);
                PageControls = NowPage.ToUIElementCollection();
            }
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

        #endregion Method
    }
}
