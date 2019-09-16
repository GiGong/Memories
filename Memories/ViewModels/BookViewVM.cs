using Memories.Views;
using Prism.Regions;

namespace Memories.ViewModels
{
    public class BookViewVM
    {
        public BookViewVM(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("BookLeftRegion", typeof(BookPageView));
            regionManager.RegisterViewWithRegion("BookRightRegion", typeof(BookPageView));
        }
    }
}
