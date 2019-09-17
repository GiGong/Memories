using Memories.Core;
using Memories.Modules.NewBook.ViewModels;
using Prism.Regions;

namespace Memories.Modules.NewBook.Views
{
    /// <summary>
    /// Interaction logic for NewBookView.xaml
    /// </summary>
    public partial class NewBookView
    {
        public NewBookView(IRegionManager regionManager)
        {
            InitializeComponent();

            IRegionManager _regionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(this, _regionManager);

            (DataContext as NewBookViewVM).RegionManager = _regionManager;
        }
    }
}
