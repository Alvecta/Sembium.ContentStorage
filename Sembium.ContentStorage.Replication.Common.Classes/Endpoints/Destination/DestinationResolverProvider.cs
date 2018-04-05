using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Destination
{
    public class DestinationResolverProvider : IDestinationResolverProvider
    {
        private readonly IEnumerable<IDestinationResolver> _eventualDestinationEndpointResolvers;

        public DestinationResolverProvider(IEnumerable<IDestinationResolver> eventualDestinationEndpointResolvers)
        {
            _eventualDestinationEndpointResolvers = eventualDestinationEndpointResolvers;
        }

        public IDestinationResolver GetResolver(IEndpointConfig config)
        {
            return _eventualDestinationEndpointResolvers.Where(x => x.CanResolve(config)).Single();
        }
    }
}
