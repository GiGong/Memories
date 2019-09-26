using Memories.Business.Enums;
using Memories.Services.Interfaces;
using Prism.Regions;
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

        private ObservableCollection<object> _layouts;
        private int? _selectedIndex;

        #endregion Field

        #region Property

        public ObservableCollection<object> Layouts
        {
            get { return _layouts; }
            set { SetProperty(ref _layouts, value); }
        }

        public int? SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SetProperty(ref _selectedIndex, value);
                if (value == null || value == -1)
                {
                    _naviParam.IsCompleted[VIEW_INDEX] = false;
                }
                else
                {
                    _naviParam.IsCompleted[VIEW_INDEX] = true;
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

            Layouts = new ObservableCollection<object>(_bookLayoutService.Load(path));
        }

        #endregion Method
    }
}
