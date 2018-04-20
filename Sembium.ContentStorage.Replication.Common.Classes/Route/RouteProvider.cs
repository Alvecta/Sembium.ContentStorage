using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public class RouteProvider : IRouteProvider
    {
        private readonly ISourceResolverProvider _sourceResolverProvider;
        private readonly IDestinationResolverProvider _destinationResolverProvider;
        private readonly IRouteFactory _routeFactory;

        public RouteProvider(
            ISourceResolverProvider sourceResolverProvider,
            IDestinationResolverProvider destinationResolverProvider,
            IRouteFactory contentsRouteFactory)
        {
            _sourceResolverProvider = sourceResolverProvider;
            _destinationResolverProvider = destinationResolverProvider;
            _routeFactory = contentsRouteFactory;
        }

        public IRoute GetRoute(IRouteConfig routeConfig)
        {
            if (routeConfig.SourceConfig == null)
                RouteNotSpecifiedError("Invalid source!");

            if (routeConfig.DestinationConfig == null)
                RouteNotSpecifiedError("Invalid destination!");

            var sourceResolver = _sourceResolverProvider.GetResolver(routeConfig.SourceConfig);
            var destinationResolver = _destinationResolverProvider.GetResolver(routeConfig.DestinationConfig);

            var source = sourceResolver.GetSource(routeConfig.SourceConfig);
            var destination = destinationResolver.GetDestination(routeConfig.DestinationConfig);

            return _routeFactory(source, destination);
        }

        private void RouteNotSpecifiedError(string message)
        {
            throw new RouteNotSpecifiedException(message);
        }
    }
}
