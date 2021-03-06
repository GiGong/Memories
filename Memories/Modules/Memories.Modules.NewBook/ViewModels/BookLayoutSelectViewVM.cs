﻿using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Converters;
using Memories.Services.Interfaces;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Memories.Modules.NewBook.ViewModels
{
    public class BookLayoutSelectViewVM : NavigationViewModelBase
    {
        public const int VIEW_INDEX = 1;

        #region Field

        private PaperSize? _paperSize;

        private readonly IFolderService _folderService;
        private readonly IBookLayoutService _bookLayoutService;
        private NewBookNavigationParameter _naviParam;

        private ObservableCollection<BookLayout> _layouts;
        private BookLayout _selectedItem;

        #endregion Field

        #region Property

        public ObservableCollection<BookLayout> Layouts
        {
            get { return _layouts; }
            set { SetProperty(ref _layouts, value); }
        }

        public BookLayout SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (value == null)
                {
                    _naviParam.IsCompleted[VIEW_INDEX] = false;
                }
                else
                {
                    _naviParam.IsCompleted[VIEW_INDEX] = true;

                    _naviParam.InputBook.BookPages = SelectedItem.Pages;
                    _naviParam.InputBook.FrontCover = SelectedItem.FrontCover;
                    _naviParam.InputBook.BackCover = SelectedItem.BackCover;
                }
            }
        }

        #endregion Property

        #region Constructor

        public BookLayoutSelectViewVM(IFolderService folderService, IBookLayoutService bookLayoutService)
        {
            _folderService = folderService;
            _bookLayoutService = bookLayoutService;

            _paperSize = null;
        }

        #endregion Constructor

        #region Method

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("Parameter", _naviParam);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var paperSize = (PaperSize)navigationContext.Parameters[nameof(PaperSize)];
            _naviParam = navigationContext.Parameters["Parameter"] as NewBookNavigationParameter;
            _naviParam.NowPage = VIEW_INDEX;
            _naviParam.ControlState = "템플릿 선택하기";

            if (_paperSize == null || _paperSize != paperSize)
            {
                _paperSize = paperSize;
                GetLayoutTemplates(paperSize);
            }
        }

        private void GetLayoutTemplates(PaperSize paperSize)
        {
            string path = _folderService.GetBookTemplateFolder(paperSize.ToString());

            Layouts = new ObservableCollection<BookLayout>(_bookLayoutService.LoadLayoutsFromDirectory(path));

            BitmapSource image = new BitmapImage(new Uri("pack://application:,,,/Resources/Img/MemoriesWhite.jpg"));
            Layouts.Insert(0, new BookLayout
            {
                Name = "빈 책",
                PreviewSource = ByteArrayToImageSourceConverter.SourceToByteArray(image)
            });
        }

        #endregion Method
    }
}
