using Memories.Core;
using Memories.Modules.Start.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Memories.Modules.Start
{
    public class StartModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public StartModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(StartView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}