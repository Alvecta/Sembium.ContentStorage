using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public interface ITransferServiceProvider
    {
        string GetURL(string containerName, string contentName, string operation, int expirySeconds);
    }
}
