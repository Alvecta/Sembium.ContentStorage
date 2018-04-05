using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public interface IContentStorage
    {
        void CreateContainer(string containerName);
        IEnumerable<string> GetContainerNames();
        IEnumerable<IContainerState> GetContainerStates();
        void SetContainerReadOnlyState(string containerName, bool readOnly);
        IEnumerable<string> Maintain(CancellationToken cancellationToken);
        IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken);
    }
}
