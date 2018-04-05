using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public interface ISignedURLProvider
    {
        string GetSignedURL(string url, DateTimeOffset startMoment, DateTimeOffset expiryMoment);
        void CheckSignedURL(string url);
    }
}
