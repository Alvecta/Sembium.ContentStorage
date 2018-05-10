using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Source;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Source
{
    public class LoggingContentStorageSource : LoggingSource, ILoggingContentStorageSource
    {
        private readonly IContentStorageSource _contentStorageSource;
        private readonly ILogger _logger;

        public LoggingContentStorageSource(IContentStorageSource source, ILogger logger)
            : base(source, logger)
        {
            _contentStorageSource = source;
            _logger = logger;
        }

        public IDownloadInfo GetDownloadInfo(IContentIdentifier contentIdentifier)
        {
            var result = _contentStorageSource.GetDownloadInfo(contentIdentifier);

            Logger.LogTrace($"Content size: {contentIdentifier.Hash} ==> {result.Size}");

            return result;
        }
    }
}
