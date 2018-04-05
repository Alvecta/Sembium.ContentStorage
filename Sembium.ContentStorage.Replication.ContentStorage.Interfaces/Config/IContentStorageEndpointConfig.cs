using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Config
{
    public interface IContentStorageEndpointConfig : IEndpointConfig
    {
        string ContainerName { get; }
        string AuthenticationToken { get; }
    }
}
