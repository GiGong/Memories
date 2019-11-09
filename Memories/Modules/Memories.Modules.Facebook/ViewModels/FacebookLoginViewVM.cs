using Facebook;
using Memories.Core;
using Memories.Services.Interfaces;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Prism.Services.Dialogs;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Windows;

namespace Memories.Modules.Facebook.ViewModels
{
    public class FacebookLoginViewVM : DialogViewModelBase
    {
        #region Field

        private readonly string _login;
        private readonly uint _state;

        private readonly IFacebookService _facebookService;

        #endregion Field

        #region Property

        public WebView WebView { get; set; }

        #endregion Property

        #region Constructor

        public FacebookLoginViewVM(IFacebookService facebookService)
        {
            _facebookService = facebookService;
            _facebookService.ClearAuthorize();

            _state = (uint)new Random().Next();

            var appSetting = ConfigurationManager.AppSettings;
            FacebookClient client = new FacebookClient()
            {
                Version = appSetting["Version"],
                AppId = appSetting["App_Id"]
            };

            _login = client.GetLoginUrl(new
            {
                redirect_uri = appSetting["RedirectUri"],
                state = _state,
                response_type = "token"
            }).AbsoluteUri;

            Title = (string)Application.Current.Resources["Designed_Program_Name"];
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            WebView.NavigationCompleted += OnNavigateCompleted;
            WebView.Loaded += (s, e) => WebView.Navigate(_login);
        }

        internal void OnNavigateCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            string urlStr = e.Uri.AbsoluteUri;
            if (!string.IsNullOrWhiteSpace(urlStr) && urlStr.StartsWith(ConfigurationManager.AppSettings["RedirectUri"], StringComparison.OrdinalIgnoreCase))
            {
                NameValueCollection urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment);

                if (urlParams.Count == 0)
                {
                    RaiseRequestClose(new DialogResult(ButtonResult.OK));
                    return;
                }

                if (!urlParams.AllKeys.Contains("state") || !uint.TryParse(urlParams.Get("state"), out uint state) || _state != state)
                {
                    throw new Exception("Facebook Server Error!" + Environment.NewLine + urlStr + " doesn't return variable!");
                }

                if (urlParams.AllKeys.Contains("#access_token"))
                {
                    string token = urlParams.Get("#access_token");
                    try
                    {
                        _facebookService.Authorize(token);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    FacebookClient client = new FacebookClient(token);
                    string logout = client.GetLogoutUrl(new
                    {
                        next = ConfigurationManager.AppSettings["RedirectUri"],
                        access_token = token
                    }).AbsoluteUri;
                    WebView.Navigate(logout);
                }
            }
        }

        #endregion Method
    }
}
