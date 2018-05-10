using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class RouteConfig : IRouteConfig
    {
        public IEndpointConfig SourceConfig { get; }
        public IEndpointConfig DestinationConfig { get; }
        public int ContentCountLimit { get; }
        public int ConnectionCountLimit { get; }
        public bool ForceAllContents { get; }
        public bool SkipDestinationCheck { get; }
        public bool ParallelGetLists { get; }
        public DateTimeOffset? HashCheckMoment { get; }

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
