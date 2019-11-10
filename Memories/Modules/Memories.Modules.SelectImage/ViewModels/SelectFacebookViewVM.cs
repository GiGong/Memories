using Memories.Business.Facebook;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Memories.Modules.SelectImage.ViewModels
{
    public class SelectFacebookViewVM : BindableBase
    {
        private bool _isLogin;
        private string _updatedTime;
        private FacebookPhoto _selectedFacebookPhoto;
        private List<FacebookPhoto> _photos;
        private ImageParameter _selectedImage;

        private DelegateCommand _loginFacebookCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _logoutCommand;

        private readonly IDialogService _dialogService;
        private readonly IFacebookService _facebookService;

        #region Property

        public bool IsLogin
        {
            get { return _isLogin; }
            set { SetProperty(ref _isLogin, value); }
        }

        public string UpdatedTime
        {
            get { return _updatedTime; }
            set { SetProperty(ref _updatedTime, value); }
        }

        public FacebookPhoto SelectedFacebookPhoto
        {
            get { return _selectedFacebookPhoto; }
            set
            {
                SetProperty(ref _selectedFacebookPhoto, value);
                SelectedFacebookPhotoChanged(SelectedFacebookPhoto);
            }
        }

        public List<FacebookPhoto> Photos
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }

        public ImageParameter SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand LoginFacebookCommand =>
            _loginFacebookCommand ?? (_loginFacebookCommand = new DelegateCommand(ExecuteLoginFacebookCommand));

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(ExecuteRefreshCommand));

        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(ExecuteLogoutCommand));

        #endregion Command

        #region Constructor

        public SelectFacebookViewVM(IDialogService dialogService, IFacebookService facebookService)
        {
            _dialogService = dialogService;
            _facebookService = facebookService;

            IsLogin = ConfigurationManager.AppSettings.AllKeys.Contains("token");
            if (IsLogin == true)
            {
                GetPhotos(false);
            }
        }

        #endregion Constructor

        private void ExecuteLoginFacebookCommand()
        {
            _dialogService.ShowFacebookLoginDialog(null, LoginCompleted);
        }

        private void ExecuteRefreshCommand()
        {
            GetPhotos(true);
        }

        private void ExecuteLogoutCommand()
        {
            _facebookService.ClearAuthorize();
            IsLogin = false;
        }

        private void LoginCompleted(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                IsLogin = true;
                GetPhotos(true);
            }
        }

        private void GetPhotos(bool isRefresh)
        {
            if (isRefresh)
            {
                //TODO: progress window 고려
            }

            Photos = _facebookService.GetPhotos(isRefresh)?.ToList();
            if (Photos == null)
            {
                MessageBox.Show("사진이 없습니다.");
            }

            UpdatedTime = _facebookService.GetPhotoUpdatedTime() + " 기준";
        }

        private void SelectedFacebookPhotoChanged(FacebookPhoto photo)
        {
            if (photo != null)
            {
                SelectedImage.SetSourceFromUrl(photo.SourceImage);
            }
        }
    }
}
