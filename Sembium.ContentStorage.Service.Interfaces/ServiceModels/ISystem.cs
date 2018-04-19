using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public interface ISystem
    {
        IEnumerable<string> GetContainerNames(string authenticationToken);
        void CreateContainer(string containerName, string authenticationToken);
        Task<string> MaintainContainerAsync(string containerName, string authenticationToken, CancellationToken cancellationToken);
        Task CompactContainerContentNamesAsync(string containerName, string authenticationToken, CancellationToken cancellationToken);
        IEnumerable<IContainerState> GetContainerStates(string authenticationToken);
        IEnumerable<string> GetReadOnlyContainerNames(string authenticationToken);
        void SetContainerReadOnlyState(string containerName, bool readOnly, string authenticationToken);

        IEnumerable<string> Maintain(string authenticationToken, CancellationToken cancellationToken);

        IEnumerable<string> GetContainerReadOnlySubcontainerNames(string containerName, string authenticationToken);
        void AddContainerReadOnlySubcontainer(string containerName, string subcontainerName, string authenticationToken);
        void RemoveContainerReadOnlySubcontainer(string containerName, string subcontainerName, string authenticationToken);
    }
}
