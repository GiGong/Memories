using Prism.Mvvm;
using Prism.Regions;

namespace Memories.Modules.NewBook.ViewModels
{
    public abstract class NewBookViewModelBase : BindableBase, INavigationAware
    {
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
