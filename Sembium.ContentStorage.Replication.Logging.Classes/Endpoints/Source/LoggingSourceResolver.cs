using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Source
{
    public class LoggingSourceResolver : ISourceResolver
    {
        private readonly ISourceResolver _sourceResolver;
        private readonly ILoggingSourceFactory _loggingSourceFactory;
        private readonly ILoggingContentStorageSourceFactory _loggingContentStorageSourceFactory;

        public LoggingSourceResolver(
            ISourceResolver sourceResolver,
            ILoggingSourceFactory loggingSourceFactory,
            ILoggingContentStorageSourceFactory loggingContentStorageSourceFactory)
        {
            _sourceResolver = sourceResolver;
            _loggingSourceFactory = loggingSourceFactory;
            _loggingContentStorageSourceFactory = loggingContentStorageSourceFactory;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return _sourceResolver.CanResolve(config);
        }

        public ISource GetSource(IEndpointConfig config)
        {
            var source = _sourceResolver.GetSource(config);

            if (source is IContentStorageSource)
            {
                return _loggingContentStorageSourceFactory(source as IContentStorageSource);
            }

            return _loggingSourceFactory(source);
        }
    }
}
