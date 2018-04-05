using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Config
{
    public class ContentStorageEndpointConfig : IContentStorageEndpointConfig
    {
        public string ContainerName { get; private set; }
        public string AuthenticationToken { get; private set; }

        public ContentStorageEndpointConfig(string containerName, string authenticationToken)
        {
            ContainerName = containerName;
            AuthenticationToken = authenticationToken;
        }
    }
}
