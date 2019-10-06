using Memories.Business.Models;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        #region Field

        private ObservableCollection<UIElement> _pageControls;
        private byte[] _background;
        private BookPage _nowPage;

        private DelegateCommand<Image> _imageClickCommand;
        private DelegateCommand<Canvas> _canvasClickCommand;

        private readonly IFileService _fileService;

        #endregion Field

        #region Property

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

                Background = NowPage == null ? null : NowPage.Background;
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
            _imageClickCommand ?? (_imageClickCommand = new DelegateCommand<Image>(ExecuteSelectImageCommand));

        public DelegateCommand<Canvas> CanvasClickCommand =>
            _canvasClickCommand ?? (_canvasClickCommand = new DelegateCommand<Canvas>(ExecuteSelectImageCommand));

        #endregion Command

        #region Method


        private UIElement BookUIToUIElement(BookUI source, int index)
        {
            if (source.UIType == Business.Enums.BookUIEnum.TextUI)
            {
                var richTextBox = (source as BookTextUI).ToRichTextBox();
                richTextBox.SetBinding(Xceed.Wpf.Toolkit.RichTextBox.TextProperty,
                    new Binding($"NowPage.PageControls[{index}].Document")
                    { Mode = BindingMode.TwoWay });
                return richTextBox;
            }
            else if (source.UIType == Business.Enums.BookUIEnum.ImageUI)
            {
                var image = (source as BookImageUI).ToImage();
                image.SetBinding(Image.SourceProperty,
                    new Binding($"NowPage.PageControls[{index}].ImageSource")
                    {
                        Mode = BindingMode.TwoWay,
                        Converter = new ByteArrayToImageSourceConverter(),
                        TargetNullValue = new BitmapImage(new Uri("pack://application:,,,/Resources/Img/MemoriesEmptyImage.jpg"))
                    });
                return image;
            }
            else
            {
                throw new ArgumentOutOfRangeException(source + " is not BookUI");
            }
        }

        private void ExecuteSelectImageCommand(FrameworkElement frameworkElement)
        {
            if (!(frameworkElement is Image || frameworkElement is Canvas))
            {
                throw new ArgumentException(frameworkElement + " is not image or canvas.");
            }

            string filter = "Image Files|*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.bmp;*.dib;*.gif|All files|*.*";
            string path = _fileService.OpenFilePath(filter);

            if (path == null)
            {
                return;
            }

            var bitmap = new BitmapImage(new Uri(path));
            try
            {
                if (frameworkElement is Image image)
                {
                    image.Source = bitmap;
                }
                else if (frameworkElement is Canvas canvas)
                {
                    (canvas.Background as ImageBrush).ImageSource = bitmap;
                }
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("Not supported file\n지원하지 않는 파일입니다.", "Memories", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Method
    }
}
