using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class MergeRouteConfigProvider : IMergeRouteConfigProvider
    {
        private readonly IRouteConfigFactory _routeConfigFactory;

        public MergeRouteConfigProvider(IRouteConfigFactory routeConfigFactory)
        {
            _routeConfigFactory = routeConfigFactory;
        }

        public IRouteConfig GetRouteConfig(IRouteConfig routeConfig1, IRouteConfig routeConfig2)
        {
            return
                _routeConfigFactory(
                    routeConfig2.SourceConfig ?? routeConfig1.SourceConfig,
                    routeConfig2.DestinationConfig ?? routeConfig1.DestinationConfig,
                    Math.Max(routeConfig2.ContentCountLimit, routeConfig1.ContentCountLimit),
                    Math.Max(routeConfig2.ConnectionCountLimit, routeConfig1.ConnectionCountLimit),
                    routeConfig2.ForceAllContents || routeConfig1.ForceAllContents,
                    routeConfig2.SkipDestinationCheck || routeConfig1.SkipDestinationCheck,
                    routeConfig2.ParallelGetLists || routeConfig1.ParallelGetLists,
                    routeConfig2.HashCheckMoment ?? routeConfig1.HashCheckMoment
                );
        }
    }
}
