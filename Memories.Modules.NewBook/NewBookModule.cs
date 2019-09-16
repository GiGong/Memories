using Memories.Core;
using Memories.Modules.NewBook.ViewModels;
using Memories.Modules.NewBook.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Memories.Modules.NewBook
{
    public class NewBookModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public NewBookModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MakeBookRegion, typeof(InputBookInfoView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<NewBookView, NewBookViewVM>();
            containerRegistry.RegisterForNavigation<LayoutSelectView, LayoutSelectViewVM>();
        }
    }
}