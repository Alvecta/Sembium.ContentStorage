using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public class SourceResolverProvider : ISourceResolverProvider
    {
        private readonly IEnumerable<ISourceResolver> _eventualSourceEndpointResolvers;

        public SourceResolverProvider(IEnumerable<ISourceResolver> eventualSourceEndpointResolvers)
        {
            _eventualSourceEndpointResolvers = eventualSourceEndpointResolvers;
        }

        public ISourceResolver GetResolver(IEndpointConfig config)
        {
            return _eventualSourceEndpointResolvers.Where(x => x.CanResolve(config)).Single();
        }
    }
}
