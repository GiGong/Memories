using Prism.Services.Dialogs;
using System;

namespace Memories.Core.Extensions
{
    public static class DialogServiceExtension
    {
        public static void ShowNewBookDialog(this IDialogService dialogService, IDialogParameters parameters, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("NewBookView", parameters, callBack);
        }

        public static void ShowEditBook(this IDialogService dialogService, IDialogParameters parameters, Action<IDialogResult> callBack)
        {
            dialogService.Show("EditBookView", parameters, callBack);
        }

        public static void ShowSelectImageDialog(this IDialogService dialogService, IDialogParameters parameters, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("SelectImageView", parameters, callBack);
        }

        public static void ShowFacebookLoginDialog(this IDialogService dialogService, IDialogParameters parameters, Action<IDialogResult> callBack)
        {
            dialogService.ShowDialog("FacebookLoginView", parameters, callBack);
        }
    }
}
