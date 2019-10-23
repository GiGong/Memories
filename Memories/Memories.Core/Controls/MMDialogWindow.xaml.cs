using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Core.Controls
{
    /// <summary>
    /// Interaction logic for MMDialogWindow.xaml
    /// </summary>
    public partial class MMDialogWindow : Window, IDialogWindow
    {
        public IDialogResult Result { get; set; }


        public MMDialogWindow()
        {
            InitializeComponent();
        }

    }
}
