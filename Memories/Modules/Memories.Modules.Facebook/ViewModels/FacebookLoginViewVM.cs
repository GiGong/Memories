using Facebook;
using Memories.Core;
using Memories.Modules.Facebook.Data;
using Memories.Services.Interfaces;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
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
                response_type = "token",
                scope = "user_photos"
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
                NameValueCollection urlParams = HttpUtility.ParseQueryString(e.Uri.Query + e.Uri.Fragment);

                if (urlParams.Count == 0)
                {
                    RaiseRequestClose(new DialogResult(ButtonResult.OK));
                    return;
                }

                if (urlParams.AllKeys.Contains("state") && uint.TryParse(urlParams.Get("state"), out uint state) && _state != state)
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

                    FacebookClient client = new FacebookClient(token)
                    {
                        Version = ConfigurationManager.AppSettings["Version"]
                    };

                    string permission = client.Get("me", new { fields = "permissions" }).ToString();
                    string data = JObject.Parse(permission).SelectToken("permissions").SelectToken("data").ToString();
                    List<FacebookPermission> permissions = JsonConvert.DeserializeObject<List<FacebookPermission>>(data);
                    List<string> declined = new List<string>();
                    foreach (var item in permissions)
                    {
                        if (item.Status == "declined")
                        {
                            declined.Add(item.Permission);
                        }
                    }

                    if (declined.Count > 0)
                    {
                        if (declined.Contains("user_photos"))
                        {
                            var result = MessageBox.Show("사진 권한을 허용하지 않으면 앱에서 Facebook을 사용할 수 없습니다." + Environment.NewLine
                                + "허용하시겠습니까?", "Memories", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                ReRequest(declined, token);
                                return;
                            }
                            else
                            {
                                _facebookService.ClearAuthorize();
                                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                                return;
                            }
                        }
                    }

                    string logout = client.GetLogoutUrl(new
                    {
                        next = ConfigurationManager.AppSettings["RedirectUri"],
                        access_token = token
                    }).AbsoluteUri;
                    WebView.Navigate(logout);
                }

                if (urlParams.AllKeys.Contains("error"))
                {
                    string error = urlParams.Get("error");
                    if (error == "access_denied")
                    {
                        RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                        return;
                    }
                }
            }
        }

        private void ReRequest(List<string> scopes, string token)
        {
            var appSetting = ConfigurationManager.AppSettings;
            FacebookClient client = new FacebookClient(token)
            {
                Version = ConfigurationManager.AppSettings["Version"],
                AppId = appSetting["App_Id"]
            };

            StringBuilder sb = new StringBuilder();
            foreach (var item in scopes)
            {
                sb.Append(item + ",");
            }

            string request = client.GetLoginUrl(new
            {
                redirect_uri = appSetting["RedirectUri"],
                response_type = "token",
                auth_type = "rerequest",
                scope = sb.ToString()
            }).AbsoluteUri;

            WebView.Navigate(request);
        }

        #endregion Method
    }
}
