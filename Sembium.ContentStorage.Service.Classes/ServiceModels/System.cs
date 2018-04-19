using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public class System : ISystem
    {
        private readonly IContentStorageAccountProvider _contentStorageAccountProvider;

        public System(
            IContentStorageAccountProvider contentStorageAccountProvider)
        {
            _contentStorageAccountProvider = contentStorageAccountProvider;
        }

        public IEnumerable<string> GetContainerNames(string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            return contentStorage.GetContainerNames();
        }

        public void CreateContainer(string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            contentStorage.CreateContainer(containerName);
        }

        public IEnumerable<IContainerState> GetContainerStates(string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            return contentStorage.GetContainerStates();
        }

        public IEnumerable<string> GetReadOnlyContainerNames(string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            return contentStorage.GetContainerStates().Where(x => x.IsReadOnly).Select(x => x.ContainerName);
        }

        public void SetContainerReadOnlyState(string containerName, bool readOnly, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            contentStorage.SetContainerReadOnlyState(containerName, readOnly);
        }

        public IEnumerable<string> GetContainerReadOnlySubcontainerNames(string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetReadOnlySubcontainerNames();
        }

        public void AddContainerReadOnlySubcontainer(string containerName, string subcontainerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            contentStorageContainer.AddReadOnlySubcontainer(subcontainerName);
        }

        public void RemoveContainerReadOnlySubcontainer(string containerName, string subcontainerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            contentStorageContainer.RemoveReadOnlySubcontainer(subcontainerName);
        }

        public async Task<string> MaintainContainerAsync(string containerName, string prefix, string authenticationToken, CancellationToken cancellationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return await contentStorageContainer.MaintainAsync(prefix, cancellationToken);
        }

        public IEnumerable<string> Maintain(string authenticationToken, CancellationToken cancellationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorage = account.GetContentStorage();

            return contentStorage.Maintain(cancellationToken);
        }

        public async Task CompactContainerContentNamesAsync(string containerName, string authenticationToken, CancellationToken cancellationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            await contentStorageContainer.CompactContentNamesAsync(cancellationToken);
        }
    }
}