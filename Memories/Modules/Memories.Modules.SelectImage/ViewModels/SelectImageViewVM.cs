using Memories.Core;
using Memories.Core.Names;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.SelectImage.ViewModels
{
    public class SelectImageViewVM : DialogViewModelBase
    {
        private ImageParameter _selectedImage;

        private DelegateCommand _cancelCommand;
        private DelegateCommand _acceptCommand;

        private byte[] _originalImage;
        public byte[] OriginalImage
        {
            get { return _originalImage; }
            set { SetProperty(ref _originalImage, value); }
        }

        public ImageParameter SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancelCommand));

        public DelegateCommand AcceptCommand =>
            _acceptCommand ?? (_acceptCommand = new DelegateCommand(ExecuteAcceptCommand));


        public SelectImageViewVM()
        {

        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            OriginalImage = parameters?.GetValue<byte[]>(ParameterNames.SelectedImage);
            SelectedImage = new ImageParameter();
        }

        private void ExecuteCancelCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        private void ExecuteAcceptCommand()
        {
            DialogParameters parameters = new DialogParameters
            {
                { ParameterNames.SelectedImage, SelectedImage.Source }
            };

            RaiseRequestClose(new DialogResult(ButtonResult.OK, parameters));
        }
    }
}
