using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IContentStorageHost
    {
        bool ContainerExists(string containerName);
        IContainer CreateContainer(string containerName);
        IContainer GetContainer(string containerName, bool createIfNotExists = false);
        IEnumerable<string> GetContainerNames();
        IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken);
    }
}
