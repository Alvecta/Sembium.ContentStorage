using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source
{
    public delegate IContentStorageSource IContentStorageSourceFactory(string containerName, string authenticationToken, string contentStorageServiceURL);
}
