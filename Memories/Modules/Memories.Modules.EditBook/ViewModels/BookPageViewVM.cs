using Memories.Business;
using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
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
        private DelegateCommand<MMRichTextBox> _textBoxGotKeyboardFocusCommand;
        private DelegateCommand<FrameworkElement> _imageSelectCommand;
        private DelegateCommand<object> _drawControlCommand;
        private DelegateCommand<Rectangle> _drawEndCommand;
        private DelegateCommand<BookUI> _deleteUICommand;

        private readonly IFileService _fileService;
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

        #region Constructor

        public BookPageViewVM(IFileService fileService, IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _fileService = fileService;
            _eventAggregator = eventAggregator;

            applicationCommands.DrawControlCommand.RegisterCommand(DrawControlCommand);

            _eventAggregator.GetEvent<DrawControlEndedEvent>().Subscribe(DrawAlreadyEnded);

            _newUI = null;
        }

        #endregion Constructor

        #region Command

        public DelegateCommand<MMRichTextBox> TextBoxGotKeyboardFocusCommand =>
            _textBoxGotKeyboardFocusCommand ?? (_textBoxGotKeyboardFocusCommand = new DelegateCommand<MMRichTextBox>(ExecuteTextBoxGotKeyboardFocusCommand));

        public DelegateCommand CanvasCommand =>
            _canvasCommand ?? (_canvasCommand = new DelegateCommand(ExecuteCanvasCommand));

        public DelegateCommand<FrameworkElement> ImageSelectCommand =>
            _imageSelectCommand ?? (_imageSelectCommand = new DelegateCommand<FrameworkElement>(ExecuteSelectImageCommand, CanExecuteSelectImageCommand));

        public DelegateCommand<object> DrawControlCommand =>
            _drawControlCommand ?? (_drawControlCommand = new DelegateCommand<object>(ExecuteDrawControlCommand));

        public DelegateCommand<Rectangle> DrawEndCommand =>
            _drawEndCommand ?? (_drawEndCommand = new DelegateCommand<Rectangle>(ExecuteDrawEndCommand));

        public DelegateCommand<BookUI> DeleteUICommand =>
            _deleteUICommand ?? (_deleteUICommand = new DelegateCommand<BookUI>(ExecuteDeleteUICommand));

        #endregion Command

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

        private bool CanExecuteSelectImageCommand(FrameworkElement frameworkElement)
        {
            return frameworkElement is MMCenterImage || frameworkElement is Canvas;
        }

        private void ExecuteSelectImageCommand(FrameworkElement frameworkElement)
        {
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
                MessageBox.Show("Not supported file" + Environment.NewLine + "지원하지 않는 파일입니다.", "Memories", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
