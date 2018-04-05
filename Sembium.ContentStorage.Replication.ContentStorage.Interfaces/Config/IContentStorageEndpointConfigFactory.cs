using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Config
{
    public delegate IContentStorageEndpointConfig IContentStorageEndpointConfigFactory(string containerName, string authenticationToken);
}
