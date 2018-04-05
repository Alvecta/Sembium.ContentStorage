using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Route;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Route
{
    public class LoggingRouteReplicator : IRouteReplicator
    {
        private readonly IRouteReplicator _routeReplicator;
        private readonly ILogger _logger;

        public LoggingRouteReplicator(IRouteReplicator routeReplicator, ILogger logger)
        {
            _routeReplicator = routeReplicator;
            _logger = logger;
        }

        public void ReplicateRouteContents(IRoute route, IEnumerable<IContentIdentifier> contentIdentifiers, int connectionCountLimit)
        {
            var contentIdentifiersList = contentIdentifiers.ToList();

            _logger.LogInfo($"Replicating {contentIdentifiersList.Count} contents...");

            _routeReplicator.ReplicateRouteContents(route, contentIdentifiersList, connectionCountLimit);
        }
    }
}
