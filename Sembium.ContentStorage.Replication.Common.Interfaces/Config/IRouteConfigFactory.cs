using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public delegate IRouteConfig IRouteConfigFactory(
        IEndpointConfig sourceConfig, 
        IEndpointConfig destinationConfig, 
        int contentCountLimit, 
        int connectionCountLimit,
        bool forceAllContents,
        bool skipDestinationCheck, 
        bool parallelGetLists,
        DateTimeOffset? hashCheckMoment
    );
}
