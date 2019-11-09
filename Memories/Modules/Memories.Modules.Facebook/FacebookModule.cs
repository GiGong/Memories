using Memories.Modules.Facebook.ViewModels;
using Memories.Modules.Facebook.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Memories.Modules.Facebook
{
    public class FacebookModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<FacebookLoginView, FacebookLoginViewVM>();
        }
    }
}