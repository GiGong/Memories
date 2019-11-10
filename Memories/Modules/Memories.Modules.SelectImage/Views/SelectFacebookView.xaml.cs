using Memories.Modules.SelectImage.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace Memories.Modules.SelectImage.Views
{
    /// <summary>
    /// Interaction logic for SelectFacebookView
    /// </summary>
    public partial class SelectFacebookView : UserControl
    {
        public SelectFacebookView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += SelectFacebookView_PropertyChanged;
        }

        private void SelectFacebookView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                (DataContext as SelectFacebookViewVM).SelectedImage = (ImageParameter)context.Value;
            }
        }
    }
}
