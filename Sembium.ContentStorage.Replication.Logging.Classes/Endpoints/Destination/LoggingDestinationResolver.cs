using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Destination
{
    public class LoggingDestinationResolver : IDestinationResolver
    {
        private readonly IDestinationResolver _DestinationResolver;
        private readonly ILoggingDestinationFactory _loggingDestinationFactory;

        public LoggingDestinationResolver(
            IDestinationResolver DestinationResolver,
            ILoggingDestinationFactory loggingDestinationFactory)
        {
            _DestinationResolver = DestinationResolver;
            _loggingDestinationFactory = loggingDestinationFactory;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return _DestinationResolver.CanResolve(config);
        }

        public IDestination GetDestination(IEndpointConfig config)
        {
            var Destination = _DestinationResolver.GetDestination(config);
            return _loggingDestinationFactory(Destination);
        }
    }
}
