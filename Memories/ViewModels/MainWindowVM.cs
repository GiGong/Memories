using Memories.Core;
using Memories.Views;
using Prism.Regions;

namespace Memories.ViewModels
{
    public class MainWindowVM
    {
        public MainWindowVM(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(StartView));
        }
    }
}
