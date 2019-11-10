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
        #region Cookie JavaScript

        private const string COOKIE_CLEAR = "javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())";

        #endregion Cookie JavaScript

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

            _login = _facebookService.GetLoginUrl(new
            {
                redirect_uri = ConfigurationManager.AppSettings["RedirectUri"],
                state = _state,
                response_type = "token",
                scope = "user_photos"
            });

            Title = (string)Application.Current.Resources["Designed_Program_Name"];
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            WebView.NavigationCompleted += OnNavigateCompleted;
            WebView.Loaded += (s, e) => WebView.Navigate(_login);

            base.OnDialogOpened(parameters);
        }

        public override void RaiseRequestClose(IDialogResult dialogResult)
        {
            WebView.Navigate(COOKIE_CLEAR);
            if (dialogResult?.Result == ButtonResult.Cancel)
            {
                _facebookService.ClearAuthorize();
            }

            base.RaiseRequestClose(dialogResult);
        }

        internal void OnNavigateCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            string urlStr = e.Uri.AbsoluteUri;
            if (!string.IsNullOrWhiteSpace(urlStr))
            {
                if (urlStr.StartsWith(ConfigurationManager.AppSettings["RedirectUri"], StringComparison.OrdinalIgnoreCase))
                {
                    HandleRedirectUri(HttpUtility.ParseQueryString(e.Uri.Query + e.Uri.Fragment));
                }
                else if (urlStr == "https://www.facebook.com/dialog/close")
                {
                    RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                    return;
                }
            }
        }

        private void HandleRedirectUri(NameValueCollection urlParams)
        {
            if (urlParams.AllKeys.Contains("state") && uint.TryParse(urlParams.Get("state"), out uint state) && _state != state)
            {
                throw new Exception("Facebook Server Error!");
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

                var declined = _facebookService.GetDeclinedList(token);
                if (declined != null && declined.Contains("user_photos"))
                {
                    var result = MessageBox.Show("사진 권한을 허용하지 않으면 앱에서 Facebook을 사용할 수 없습니다." + Environment.NewLine
                        + "허용하시겠습니까?", "Memories", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        WebView.Navigate(_facebookService.GetReRequestUrl(declined, token));
                        return;
                    }
                    else
                    {
                        RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                        return;
                    }
                }

                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
            else if (urlParams.AllKeys.Contains("error"))
            {
                string error = urlParams.Get("error");
                if (error == "access_denied")
                {
                    RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                    return;
                }
            }
        }

        #endregion Method
    }
}
