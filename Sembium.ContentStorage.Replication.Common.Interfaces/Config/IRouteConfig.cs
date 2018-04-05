using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public interface IRouteConfig
    {
        IEndpointConfig SourceConfig { get; }
        IEndpointConfig DestinationConfig { get; }
        int ContentCountLimit { get; }
        int ConnectionCountLimit { get; }
        bool ForceAllContents { get; }
        bool SkipDestinationCheck { get; }
        bool ParallelGetLists { get; }
        DateTimeOffset? HashCheckMoment { get; }
    }
}
