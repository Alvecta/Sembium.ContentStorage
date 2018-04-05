using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common
{
    public interface IContentStorageServiceProvider
    {
        string GetURL(string containerName);
        string GetContainerName(string containerName);
    }
}
