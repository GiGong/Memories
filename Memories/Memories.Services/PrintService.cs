using Memories.Services.Interfaces;
using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Memories.Services
{
    public class PrintService : IPrintService
    {
        public void Print(object document)
        {
            if (!(document is DocumentPaginator print))
            {
                throw new ArgumentException(document.GetType().FullName + " is not DocumentPaginator" + Environment.NewLine + "In " + nameof(IPrintService));
            }

            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintDocument(print, "Memories Book");
            }
        }
    }
}
