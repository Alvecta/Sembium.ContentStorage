using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Destination
{
    public delegate ILoggingDestination ILoggingDestinationFactory(IDestination destination);
}
