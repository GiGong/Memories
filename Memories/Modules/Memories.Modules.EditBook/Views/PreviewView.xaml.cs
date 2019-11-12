using Memories.Business.Models;
using Memories.Modules.EditBook.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace Memories.Modules.EditBook.Views
{
    /// <summary>
    /// Interaction logic for PreviewView
    /// </summary>
    public partial class PreviewView : UserControl
    {
        public PreviewView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += PreviewView_PropertyChanged;
        }

        private void PreviewView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                (DataContext as PreviewViewVM).EditBook = (Book)context.Value;
            }
        }
    }
}
