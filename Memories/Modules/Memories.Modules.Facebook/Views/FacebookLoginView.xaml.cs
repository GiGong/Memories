using Memories.Modules.Facebook.Handler;
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

            chrome.RequestHandler = new MMRequestHandler(Dispatcher, DataContext as FacebookLoginViewVM);
            (DataContext as FacebookLoginViewVM).Chromium = chrome;
        }
    }
}
