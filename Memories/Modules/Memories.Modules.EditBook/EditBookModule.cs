using Memories.Core;
using Memories.Modules.EditBook.ViewModels;
using Memories.Modules.EditBook.Views;
using Memories.Services;
using Memories.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Memories.Modules.EditBook
{
    public class EditBookModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public EditBookModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.BookRegion, typeof(BookView));
            _regionManager.RegisterViewWithRegion(RegionNames.LeftPageRegion, typeof(BookPageView));
            _regionManager.RegisterViewWithRegion(RegionNames.RightPageRegion, typeof(BookPageView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBookPageLayoutService, BookPageLayoutService>();

            containerRegistry.RegisterDialog<EditBookView, EditBookViewVM>();
            containerRegistry.RegisterDialog<PageLayoutSelectView, PageLayoutSelectViewVM>();
        }
    }
}