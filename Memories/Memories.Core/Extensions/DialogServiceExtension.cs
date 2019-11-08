using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
