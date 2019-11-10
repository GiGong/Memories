using Memories.Core;
using Memories.Modules.SelectImage.ViewModels;
using Memories.Modules.SelectImage.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Memories.Modules.SelectImage
{
    public class SelectImageModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SelectImageModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.SelectFileView, typeof(SelectFileView));
            _regionManager.RegisterViewWithRegion(RegionNames.SelectFacebookView, typeof(SelectFacebookView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SelectImageView, SelectImageViewVM>();
        }
    }
}