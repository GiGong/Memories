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
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<NewBookView, NewBookViewVM>();
            containerRegistry.RegisterForNavigation<InputBookInfoView, InputBookInfoViewVM>();
            containerRegistry.RegisterForNavigation<LayoutSelectView, LayoutSelectViewVM>();
        }
    }
}