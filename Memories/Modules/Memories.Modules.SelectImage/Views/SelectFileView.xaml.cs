using Memories.Modules.SelectImage.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;
using System.Windows.Media;

namespace Memories.Modules.SelectImage.Views
{
    /// <summary>
    /// Interaction logic for SelectFileView
    /// </summary>
    public partial class SelectFileView : UserControl
    {
        public SelectFileView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += SelectFileView_PropertyChanged;
        }

        private void SelectFileView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                (DataContext as SelectFileViewVM).SelectedImage = (ImageParameter)context.Value;
            }
        }
    }
}
