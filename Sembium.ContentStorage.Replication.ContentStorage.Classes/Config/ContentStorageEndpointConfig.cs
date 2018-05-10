using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Config
{
    public class ContentStorageEndpointConfig : IContentStorageEndpointConfig
    {
        public string ContainerName { get; }
        public string AuthenticationToken { get; }

        public ContentStorageEndpointConfig(string containerName, string authenticationToken)
        {
            ContainerName = containerName;
            AuthenticationToken = authenticationToken;
        }
    }
}
