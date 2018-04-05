using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public interface IRoute
    {
        ISource Source { get; }
        IDestination Destination { get; }
    }
}
