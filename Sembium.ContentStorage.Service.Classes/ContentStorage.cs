using Sembium.ContentStorage.Misc.Utils;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public class ContentStorage : IContentStorage
    {
        private const string SystemContainerName = "system";

        private readonly IContentStorageHost _contentStorageHost;
        private readonly IContentStorageContainerFactory _contentStorageContainerFactory;
        private readonly IContainerStateRepository _containerStateRepository;
        private readonly IAuthorizationChecker _authorizationChecker;

        public ContentStorage(
            IContentStorageHost contentStorageHost,
            IContentStorageContainerFactory contentStorageContainerFactory,
            IContainerStateRepository containerStateRepository,
            IAuthorizationChecker authorizationChecker)
        {
            _contentStorageHost = contentStorageHost;
            _contentStorageContainerFactory = contentStorageContainerFactory;
            _containerStateRepository = containerStateRepository;
            _authorizationChecker = authorizationChecker;
        }

        public void CreateContainer(string containerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            ValidateContainerName(containerName);
            _contentStorageHost.CreateContainer(containerName);
        }

        private void ValidateContainerName(string containerName)
        {
            if (string.IsNullOrEmpty(containerName) || (containerName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0))
                throw new UserException("Invalid container name: " + containerName);
        }

        public IEnumerable<string> GetContainerNames()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.Maintainer);

            return _contentStorageHost.GetContainerNames().Where(x => !IsSystemContainer(x));
        }

        private static bool IsSystemContainer(string containerName)
        {
            return containerName.StartsWith(SystemContainerName, StringComparison.InvariantCultureIgnoreCase);
        }

        public IEnumerable<IContainerState> GetContainerStates()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.System);

            return _containerStateRepository.GetContainerStates();
        }

        public void SetContainerReadOnlyState(string containerName, bool readOnly)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            _containerStateRepository.SetContainerStateAsync(containerName, readOnly, null).Wait();
        }

        public IEnumerable<string> Maintain(CancellationToken cancellationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.Maintainer);

            foreach (var containerName in GetContainerNames())
            {
                yield return MaintainContainer(containerName, cancellationToken);
            };
        }

        private string MaintainContainer(string containerName, CancellationToken cancellationToken)
        {
            try
            {
                var container = _contentStorageContainerFactory(containerName);
                return container.MaintainAsync(null, cancellationToken).Result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator);

            return _contentStorageHost.GetUrlContentUploadInfo(contentUrl, contentStorageServiceUrl, containerName, contentID, size, authenticationToken);
        }
    }
}
