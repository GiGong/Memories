using Memories.Business.Facebook;
using Memories.Core.Converters;
using Memories.Core.Extensions;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Memories.Modules.SelectImage.ViewModels
{
    public class SelectFacebookViewVM : BindableBase
    {
        private const int PHOTO_TERM = 15;
        private bool _isLoading;

        IEnumerator<IEnumerable<FacebookPhoto>> _enumerator;
        private List<FacebookPhoto> _photoData;

        private bool _isLogin;
        private string _updatedTime;
        private int _selectedIndex;
        private ObservableCollection<BitmapImage> _photos;
        private ImageParameter _selectedImage;

        private DelegateCommand _loginFacebookCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _logoutCommand;
        private DelegateCommand _loadPhotoCommand;

        private readonly IDialogService _dialogService;
        private readonly IFacebookService _facebookService;

        #region Property

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

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

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SetProperty(ref _selectedIndex, value);
                SelectedIndexChanged(SelectedIndex);
            }
        }

        public ObservableCollection<BitmapImage> Photos
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
            Photos = null;
        }

        private void LoginCompleted(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                IsLogin = true;
                GetPhotos(true);
            }
        }

        private async void GetPhotos(bool isRefresh)
        {
            IsLoading = true;
            _photoData = (await _facebookService.GetPhotosAsync(isRefresh))?.ToList();
            IsLoading = false;
            if (_photoData == null)
            {
                MessageBox.Show("사진이 없습니다.");
                return;
            }

            _enumerator = GetPhotosYield();
            Photos = new ObservableCollection<BitmapImage>();
            ExecuteLoadPhotoCommand();

            UpdatedTime = _facebookService.GetPhotoUpdatedTime() + " 기준";
        }

        private void SelectedIndexChanged(int index)
        {
            if (index < 0)
            {
                return;
            }
            SelectedImage.SetSourceFromUrl(_photoData[index].SourceImage);
        }

        private void ExecuteLoadPhotoCommand()
        {
            if (!IsLoading && (_enumerator?.MoveNext() ?? false))
            {
                AddPhoto(_enumerator.Current);
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

        private async void AddPhoto(IEnumerable<FacebookPhoto> photos)
        {
            IsLoading = true;
            WebClient wc = new WebClient();
            foreach (var photo in photos)
            {
                byte[] buffer;

                buffer = await wc.DownloadDataTaskAsync(photo.PreviewImage);

                MemoryStream ms = new MemoryStream(buffer);
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();

                Photos.Add(img);
            }
            wc.Dispose();
            IsLoading = false;
        }
    }
}
