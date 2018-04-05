using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class MergeConfigProvider : IMergeConfigProvider
    {
        private readonly IConfigFactory _configFactory;
        private readonly IMergeRouteConfigProvider _mergeRouteConfigProvider;

        public MergeConfigProvider(IConfigFactory configFactory, IMergeRouteConfigProvider mergeRouteConfigProvider)
        {
            _configFactory = configFactory;
            _mergeRouteConfigProvider = mergeRouteConfigProvider;
        }

        public IConfig GetConfig(IConfig config1, IConfig config2)
        {
            return
                _configFactory(
                    config1?.RouteConfigs.Zip(
                        config2.RouteConfigs,
                        (routeConfig1, routeConfig2) => _mergeRouteConfigProvider.GetRouteConfig(routeConfig1, routeConfig2)
                    ),
                    config2?.LogFileName ?? config1?.LogFileName
                );
        }
    }
}
