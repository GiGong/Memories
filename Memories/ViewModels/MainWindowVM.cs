using Memories.Core;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Memories.ViewModels
{
    public class MainWindowVM : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        private DelegateCommand<string> _hideCommand;
        public DelegateCommand<string> HideCommand =>
            _hideCommand ?? (_hideCommand = new DelegateCommand<string>(ExecuteHideCommand));

        private DelegateCommand<string> _showCommand;
        public DelegateCommand<string> ShowCommand =>
            _showCommand ?? (_showCommand = new DelegateCommand<string>(ExecuteShowCommand));


        public MainWindowVM(IApplicationCommands applicationCommands)
        {
            Title = (string)Application.Current.Resources["Program_Name"];
            Visibility = Visibility.Visible;

            applicationCommands.HideShellCommand.RegisterCommand(HideCommand);
            applicationCommands.ShowShellCommand.RegisterCommand(ShowCommand);
        }        
        
        void ExecuteHideCommand(string param)
        {
            Visibility = Visibility.Collapsed;
        }

        void ExecuteShowCommand(string param)
        {
            Visibility = Visibility.Visible;
        }
    }
}
