using Sembium.ContentStorage.Common;
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

        public LoggingContentStorageSource(IContentStorageSource source, ILogger logger)
            : base(source, logger)
        {
            _contentStorageSource = source;
        }

        public IDownloadInfo GetDownloadInfo(IContentIdentifier contentIdentifier)
        {
            return _contentStorageSource.GetDownloadInfo(contentIdentifier);
        }
    }
}
