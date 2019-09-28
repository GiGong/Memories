using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Services.Interfaces;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Memories.Modules.NewBook.ViewModels
{
    public class LayoutSelectViewVM : NewBookViewModelBase
    {
        public const int VIEW_INDEX = 1;

        #region Field

        private readonly IFolderService _folderService;
        private readonly IBookLayoutService _bookLayoutService;
        private NewBookNavigateParameter _naviParam;

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
                }
            }
        }

        #endregion Property

        #region Constructor

        public LayoutSelectViewVM(IFolderService folderService, IBookLayoutService bookLayoutService)
        {
            _folderService = folderService;
            _bookLayoutService = bookLayoutService;
        }

        #endregion Constructor

        #region Method

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("Parameter", _naviParam);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var paperSize = (PaperSize)navigationContext.Parameters["PaperSize"];
            _naviParam = navigationContext.Parameters["Parameter"] as NewBookNavigateParameter;
            _naviParam.NowPage = VIEW_INDEX;

            GetLayoutTemplates(paperSize);
        } 

        private void GetLayoutTemplates(PaperSize paperSize)
        {
            string path = _folderService.GetAppFolder("Layouts", paperSize.ToString());

            Layouts = new ObservableCollection<BookLayout>(_bookLayoutService.LoadLayoutsFromDirectory(path));
        }

        #endregion Method
    }
}
