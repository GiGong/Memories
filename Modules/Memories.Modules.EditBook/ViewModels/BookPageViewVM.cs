﻿using Memories.Business.Models;
using Memories.Core.Converters;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        #region Field

        private ObservableCollection<UIElement> _pageControls;
        private BookPage _nowPage;

        private DelegateCommand<Image> _imageClickCommand;
        private readonly IFileService _fileService;

        #endregion Field

        #region Property

        public ObservableCollection<UIElement> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }

        public BookPage NowPage
        {
            get { return _nowPage; }
            set
            {
                SetProperty(ref _nowPage, value);

                PageControls = NowPage == null ? null : new ObservableCollection<UIElement>(NowPage.PageControls.Select(BookUIToUIElement));
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

        public DelegateCommand<Image> ImageClickCommand =>
            _imageClickCommand ?? (_imageClickCommand = new DelegateCommand<Image>(ImageClick));

        #endregion Command

        #region Method


        private UIElement BookUIToUIElement(BookUI source, int index)
        {
            if (source.UIType == Business.Enums.BookUIEnum.TextUI)
            {
                var richTextBox = (source as BookTextUI).ToRichTextBox();
                richTextBox.SetBinding(Xceed.Wpf.Toolkit.RichTextBox.TextProperty, 
                    new Binding($"NowPage.PageControls[{index}].Document")
                    { Mode = BindingMode.TwoWay});
                return richTextBox;
            }
            else if (source.UIType == Business.Enums.BookUIEnum.ImageUI)
            {
                var image = (source as BookImageUI).ToImage();
                image.SetBinding(Image.SourceProperty,
                    new Binding($"NowPage.PageControls[{index}].ImageSource")
                    { Mode = BindingMode.TwoWay, Converter = new ByteArrayToImageSourceConverter() });
                return image;
            }
            else
            {
                throw new ArgumentOutOfRangeException(source + " is not BookUI");
            }
        }

        void ImageClick(Image image)
        {
            string filter = "Image Files|*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.bmp;*.dib;*.gif|All files|*.*";

            string path = _fileService.OpenFilePath(filter);

            try
            {
                image.Source = new BitmapImage(new Uri(path));

            }
            catch (NotSupportedException)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Not supported file\n지원하지 않는 파일입니다.", "Memories", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Method
    }
}
