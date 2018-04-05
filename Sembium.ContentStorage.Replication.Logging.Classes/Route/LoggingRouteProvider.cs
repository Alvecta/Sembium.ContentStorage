using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Route
{
    public class LoggingRouteProvider : IRouteProvider
    {
        private readonly IRouteProvider _routeProvider;
        private readonly ILogger _logger;

        public LoggingRouteProvider(IRouteProvider routeProvider, ILogger logger)
        {
            _routeProvider = routeProvider;
            _logger = logger;
        }

        public IRoute GetRoute(IRouteConfig routeConfig)
        {
            _logger.LogTrace("Start getting route");

            var result = _routeProvider.GetRoute(routeConfig);

            _logger.LogTrace("End getting route");

            return result;
        }
    }
}
