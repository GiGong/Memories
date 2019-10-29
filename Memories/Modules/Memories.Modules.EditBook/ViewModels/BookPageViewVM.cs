﻿using Memories.Business;
using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Core.Converters;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Memories.Modules.EditBook.Parameters;
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

        private DelegateCommand<MMRichTextBox> _textBoxGotFocusCommand;
        private DelegateCommand<FrameworkElement> _doubleClickCommand;
        private DelegateCommand<MMCenterImage> _imageSelectCommand;
        private DelegateCommand<DrawParameter> _drawControlCommand;
        private DelegateCommand<Rectangle> _drawEndCommand;

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

        public BookPageViewVM(IFileService fileService, IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _fileService = fileService;
            _eventAggregator = eventAggregator;

            applicationCommands.DrawControlCommand.RegisterCommand(DrawControlCommand);

            _newUI = null;
        }

        #endregion Constructor

        #region Command

        public DelegateCommand<MMRichTextBox> TextBoxGotFocusCommand =>
            _textBoxGotFocusCommand ?? (_textBoxGotFocusCommand = new DelegateCommand<MMRichTextBox>(ExecuteTextBoxGotFocusCommand));

        public DelegateCommand<MMCenterImage> ImageSelectCommand =>
            _imageSelectCommand ?? (_imageSelectCommand = new DelegateCommand<MMCenterImage>(ExecuteSelectImageCommand));

        public DelegateCommand<FrameworkElement> DoubleClickCommand =>
            _doubleClickCommand ?? (_doubleClickCommand = new DelegateCommand<FrameworkElement>(ExecuteDoubleClickCommand));

        public DelegateCommand<DrawParameter> DrawControlCommand =>
            _drawControlCommand ?? (_drawControlCommand = new DelegateCommand<DrawParameter>(ExecuteDrawControlCommand));

        public DelegateCommand<Rectangle> DrawEndCommand =>
            _drawEndCommand ?? (_drawEndCommand = new DelegateCommand<Rectangle>(ExecuteDrawEndCommand));

        #endregion Command

        #region Method

        private void ExecuteTextBoxGotFocusCommand(MMRichTextBox richTextBox)
        {
            _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Publish(richTextBox);
        }

        private void ExecuteDoubleClickCommand(FrameworkElement element)
        {
            if (element is MMCenterImage || element is Canvas)
            {
                ExecuteSelectImageCommand(element);
            }
        }

        private void ExecuteDrawControlCommand(DrawParameter parameter)
        {
            if (NowPage == null)
            {
                return;
            }
            _newUI = BookUI.GetBookUI(parameter.Type);
            IsDraw = true;
        }

        private void ExecuteDrawEndCommand(Rectangle rect)
        {
            _newUI.GetPropertyFromRectangle(rect);
            NowPage.PageControls.Add(_newUI);
            PageControls = NowPage.ToUIElementCollection();
            _newUI = null;
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
