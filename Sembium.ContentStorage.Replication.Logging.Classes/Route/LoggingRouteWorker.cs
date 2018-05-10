using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Route
{
    public class LoggingRouteWorker : IRouteWorker
    {
        private readonly IRouteWorker _routeWorker;
        private readonly ILogger _logger;

        public LoggingRouteWorker(IRouteWorker routeWorker, ILogger logger)
        {
            _routeWorker = routeWorker;
            _logger = logger;
        }

        public void RunRoute(IRouteConfig routeConfig)
        {
            _routeWorker.RunRoute(routeConfig);
        }

        public void HashCheckRoute(IRouteConfig routeConfig)
        {
            try
            {
                _routeWorker.HashCheckRoute(routeConfig);

                _logger.LogInfo("Hash check succeeded.");
            }
            catch (Exception e)
            {
                _logger.LogFatal("Hash check failed.", e);
            }
        }
    }
}
