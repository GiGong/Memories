using CefSharp;
using CefSharp.Handler;
using Memories.Modules.Facebook.ViewModels;
using Prism.Services.Dialogs;
using System;
using System.Configuration;
using System.Windows.Threading;

namespace Memories.Modules.Facebook.Handler
{
    public class MMRequestHandler : RequestHandler
    {
        private readonly Dispatcher _dispatcher;
        private readonly FacebookLoginViewVM _vm;

        public MMRequestHandler(Dispatcher dispatcher, FacebookLoginViewVM vm)
        {
            _dispatcher = dispatcher;
            _vm = vm;
        }

        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            string urlStr = request.Url;
            if (!string.IsNullOrWhiteSpace(urlStr))
            {
                if (urlStr.StartsWith(ConfigurationManager.AppSettings["RedirectUri"], StringComparison.OrdinalIgnoreCase) && Uri.TryCreate(urlStr, UriKind.Absolute, out Uri accessUri))
                {
                    _dispatcher.Invoke(() => _vm.RedirectCommand.Execute(accessUri.Query + accessUri.Fragment));
                }
                else if (urlStr == "https://www.facebook.com/dialog/close")
                {
                    _dispatcher.Invoke(() => _vm.RaiseRequestClose(new DialogResult(ButtonResult.Cancel)));
                }
            }

            return base.OnBeforeBrowse(chromiumWebBrowser, browser, frame, request, userGesture, isRedirect);
        }
    }
}
