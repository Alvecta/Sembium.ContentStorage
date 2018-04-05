using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public interface ISourceResolver
    {
        ISource GetSource(IEndpointConfig config);
        bool CanResolve(IEndpointConfig config);
    }
}
