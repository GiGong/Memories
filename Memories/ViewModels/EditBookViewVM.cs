using Memories.Core;
using Memories.Views;
using Prism.Regions;

namespace Memories.ViewModels
{
    public class EditBookViewVM : DialogViewModelBase
    {

        public EditBookViewVM(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("BookRegion", typeof(BookView));
        }
    }
}
