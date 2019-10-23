using Prism.Services.Dialogs;

namespace Memories.Core.Controls
{
    /// <summary>
    /// Interaction logic for MMDialogWindow.xaml
    /// </summary>
    public partial class MMDialogWindow : IDialogWindow
    {
        public IDialogResult Result { get; set; }


        public MMDialogWindow()
        {
            InitializeComponent();
        }

    }
}
