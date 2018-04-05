using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Destination
{
    public delegate IContentStorageDestination IContentStorageDestinationFactory(string containerName, string authenticationToken, string contentStorageServiceURL);
}
