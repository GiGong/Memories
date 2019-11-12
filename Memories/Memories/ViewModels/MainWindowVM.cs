using Memories.Core;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Memories.ViewModels
{
    public class MainWindowVM : BindableBase
    {
        private string _title;
        private Visibility _visibility;

        private DelegateCommand _hideCommand;
        private DelegateCommand _showCommand;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        public DelegateCommand HideCommand =>
            _hideCommand ?? (_hideCommand = new DelegateCommand(ExecuteHideCommand));

        public DelegateCommand ShowCommand =>
            _showCommand ?? (_showCommand = new DelegateCommand(ExecuteShowCommand));


        public MainWindowVM(IApplicationCommands applicationCommands)
        {
            Title = (string)Application.Current.Resources["Program_Name"];
            Visibility = Visibility.Visible;

            applicationCommands.HideShellCommand.RegisterCommand(HideCommand);
            applicationCommands.ShowShellCommand.RegisterCommand(ShowCommand);
        }

        void ExecuteHideCommand()
        {
            Visibility = Visibility.Collapsed;
        }

        void ExecuteShowCommand()
        {
            Visibility = Visibility.Visible;
        }
    }
}
