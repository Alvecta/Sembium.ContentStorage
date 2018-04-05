using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.ContentStorage.Config;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Destination
{
    public class ContentStorageDestinationResolver : IDestinationResolver
    {
        private readonly IContentStorageDestinationFactory _contentStorageDestinationFactory;
        private readonly IContentStorageServiceProvider _contentStorageServiceProvider;

        public ContentStorageDestinationResolver(
            IContentStorageDestinationFactory contentStorageDestinationFactory,
            IContentStorageServiceProvider contentStorageServiceProvider)
        {
            _contentStorageDestinationFactory = contentStorageDestinationFactory;
            _contentStorageServiceProvider = contentStorageServiceProvider;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return (config is IContentStorageEndpointConfig);
        }

        public IDestination GetDestination(IEndpointConfig config)
        {
            if (!CanResolve(config))
                throw new ArgumentException("Missing destination config");

            var cfg = config as IContentStorageEndpointConfig;

            var contentStorageServiceURL = _contentStorageServiceProvider.GetURL(cfg.ContainerName);

            return _contentStorageDestinationFactory(_contentStorageServiceProvider.GetContainerName(cfg.ContainerName), cfg.AuthenticationToken, contentStorageServiceURL);
        }
    }
}
