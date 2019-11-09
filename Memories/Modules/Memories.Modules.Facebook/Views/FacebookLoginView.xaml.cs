using Memories.Modules.Facebook.ViewModels;
using System.Windows.Controls;

namespace Memories.Modules.Facebook.Views
{
    /// <summary>
    /// Interaction logic for FacebookLoginView
    /// </summary>
    public partial class FacebookLoginView : UserControl
    {
        public FacebookLoginView()
        {
            InitializeComponent();

            (DataContext as FacebookLoginViewVM).WebView = web;
        }
    }
}
