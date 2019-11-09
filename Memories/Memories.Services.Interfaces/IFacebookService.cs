using System.Collections.Generic;

namespace Memories.Services.Interfaces
{
    public interface IFacebookService
    {
        bool IsAuthorized { get; }

        void Authorize(string token);
        void ClearAuthorize();
    }
}
