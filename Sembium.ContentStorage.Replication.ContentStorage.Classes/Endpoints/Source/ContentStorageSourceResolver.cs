using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.ContentStorage.Config;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source
{
    public class ContentStorageSourceResolver : ISourceResolver
    {
        private readonly IContentStorageSourceFactory _contentStorageSourceFactory;
        private readonly IContentStorageServiceProvider _contentStorageServiceProvider;

        public ContentStorageSourceResolver(
            IContentStorageSourceFactory contentStorageSourceFactory,
            IContentStorageServiceProvider contentStorageServiceProvider)
        {
            _contentStorageSourceFactory = contentStorageSourceFactory;
            _contentStorageServiceProvider = contentStorageServiceProvider;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return (config is IContentStorageEndpointConfig);
        }

        public ISource GetSource(IEndpointConfig config)
        {
            if (!CanResolve(config))
                throw new ArgumentException("Missing source config");

            var cfg = config as IContentStorageEndpointConfig;

            var contentStorageServiceURL = _contentStorageServiceProvider.GetURL(cfg.ContainerName);

            return _contentStorageSourceFactory(_contentStorageServiceProvider.GetContainerName(cfg.ContainerName), cfg.AuthenticationToken, contentStorageServiceURL);
        }
    }
}
