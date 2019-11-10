using System.Collections.Generic;
using Memories.Business.Facebook;

namespace Memories.Services.Interfaces
{
    public interface IFacebookService
    {
        bool IsAuthorized { get; }

        string GetLoginUrl(object parameters);
        void Authorize(string token);
        void ClearAuthorize();
        IEnumerable<string> GetDeclinedList(string token);
        string GetReRequestUrl(IEnumerable<string> scopes, string token);
        IEnumerable<FacebookPhoto> GetPhotos(bool isRefresh);
        string GetPhotoUpdatedTime();
    }
}
