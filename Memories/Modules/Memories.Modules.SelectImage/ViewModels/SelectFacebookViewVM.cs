using Memories.Business.Facebook;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Memories.Modules.SelectImage.ViewModels
{
    public class SelectFacebookViewVM : BindableBase
    {
        private const int PHOTO_TERM = 20;

        IEnumerator<IEnumerable<FacebookPhoto>> _enumerator;
        private List<FacebookPhoto> _photoData;

        private bool _isLogin;
        private string _updatedTime;
        private FacebookPhoto _selectedFacebookPhoto;
        private ObservableCollection<FacebookPhoto> _photos;
        private ImageParameter _selectedImage;

        private DelegateCommand _loginFacebookCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _logoutCommand;
        private DelegateCommand _loadPhotoCommand;

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

        public ObservableCollection<FacebookPhoto> Photos
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

        public DelegateCommand LoadPhotoCommand =>
            _loadPhotoCommand ?? (_loadPhotoCommand = new DelegateCommand(ExecuteLoadPhotoCommand));


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
            _photoData = null;
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
            _photoData = _facebookService.GetPhotos(isRefresh)?.ToList();
            if (_photoData == null)
            {
                MessageBox.Show("사진이 없습니다.");
                return;
            }

            _enumerator = GetPhotosYield();
            _enumerator.MoveNext();
            Photos = new ObservableCollection<FacebookPhoto>(_enumerator.Current);

            UpdatedTime = _facebookService.GetPhotoUpdatedTime() + " 기준";
        }

        private void SelectedFacebookPhotoChanged(FacebookPhoto photo)
        {
            if (photo != null)
            {
                SelectedImage.SetSourceFromUrl(photo.SourceImage);
            }
        }

        private void ExecuteLoadPhotoCommand()
        {
            if (_enumerator?.MoveNext() ?? false)
            {
                Photos.AddRange(_enumerator.Current);
            }
        }

        private IEnumerator<IEnumerable<FacebookPhoto>> GetPhotosYield()
        {
            if (_photoData == null)
            {
                yield return null;
            }

            int index = 0;
            while (index < _photoData.Count)
            {
                List<FacebookPhoto> data = _photoData.GetRange(index, Math.Min(_photoData.Count - index, PHOTO_TERM));
                index += PHOTO_TERM;
                yield return data;
            }
        }
    }
}
