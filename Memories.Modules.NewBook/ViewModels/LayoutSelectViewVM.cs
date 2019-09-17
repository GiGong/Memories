using Memories.Core.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace Memories.Modules.NewBook.ViewModels
{
    public class LayoutSelectViewVM : NewBookViewModelBase
    {
        public const int VIEW_INDEX = 1;

        #region Field

        private NewBookNavigateParameter _naviParam;

        private ObservableCollection<int> _layouts;
        private int? _selectedIndex;

        #endregion Field

        #region Property

        public ObservableCollection<int> Layouts
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
                _naviParam.IsCompleted[VIEW_INDEX] = value == null || value == -1 ? false : true;
            }
        }

        #endregion Property

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

            switch (paperSize)
            {
                case PaperSize.A3:
                    Layouts = new ObservableCollection<int>() { 3, 1 };
                    break;
                case PaperSize.A4:
                    Layouts = new ObservableCollection<int>() { 4 };
                    break;
                default:
                    Layouts = new ObservableCollection<int>();
                    break;
            }
        } 

        #endregion Method
    }
}
