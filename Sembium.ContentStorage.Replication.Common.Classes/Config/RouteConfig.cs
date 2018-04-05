using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class RouteConfig : IRouteConfig
    {
        public IEndpointConfig SourceConfig { get; private set; }
        public IEndpointConfig DestinationConfig { get; private set; }
        public int ContentCountLimit { get; private set; }
        public int ConnectionCountLimit { get; private set; }
        public bool ForceAllContents { get; private set; }
        public bool SkipDestinationCheck { get; private set; }
        public bool ParallelGetLists { get; private set; }
        public DateTimeOffset? HashCheckMoment { get; private set; }

        public RouteConfig(IEndpointConfig sourceConfig, IEndpointConfig destinationConfig, int contentCountLimit, int connectionCountLimit, bool forceAllContents, bool skipDestinationCheck, bool parallelGetLists, DateTimeOffset? hashCheckMoment)
        {
            SourceConfig = sourceConfig;
            DestinationConfig = destinationConfig;
            ContentCountLimit = contentCountLimit;
            ConnectionCountLimit = connectionCountLimit;
            ForceAllContents = forceAllContents;
            SkipDestinationCheck = skipDestinationCheck;
            ParallelGetLists = parallelGetLists;
            HashCheckMoment = hashCheckMoment;
        }
    }
}
