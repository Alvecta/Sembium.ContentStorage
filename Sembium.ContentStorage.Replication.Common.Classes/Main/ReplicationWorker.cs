using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Route;
using Sembium.ContentStorage.Replication.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Main
{
    public class ReplicationWorker : IReplicationWorker
    {
        private readonly IRouteWorker _routeWorker;

        public ReplicationWorker(IRouteWorker routeWorker)
        {
            _routeWorker = routeWorker;
        }

        private Task RunRouteAsync(IRouteConfig routeConfig)
        {
            return Task.Factory.StartNew(() => _routeWorker.RunRoute(routeConfig));
        }

        private Task HashCheckRouteAsync(IRouteConfig routeConfig)
        {
            return Task.Factory.StartNew(() => _routeWorker.HashCheckRoute(routeConfig));
        }

        private Task ProcessRouteAsync(IRouteConfig routeConfig)
        {
            if (routeConfig.HashCheckMoment != null)
            {
                return HashCheckRouteAsync(routeConfig);
            }
            else
            {
                return RunRouteAsync(routeConfig);
            }
        }

        public void Run(IConfig config)
        {
            if ((config.RouteConfigs == null) || (!config.RouteConfigs.Any()))
                RouteNotSpecifiedError();

            Task.WhenAll(config.RouteConfigs.Select(x => ProcessRouteAsync(x))).Wait();
        }

        private void RouteNotSpecifiedError()
        {
            throw new RouteNotSpecifiedException("No routes specified!");
        }
    }
}
