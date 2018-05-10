using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Source
{
    public class LoggingSource : LoggingEndpoint, ILoggingSource
    {
        private readonly ISource _source;

        public LoggingSource(ISource source, ILogger logger)
            : base(source, logger)
        {
            _source = source;
        }

        public IContentStream GetContentStream(IContentIdentifier contentIdentifier)
        {
            Logger.LogTrace($"Start getting content: {contentIdentifier.Hash}");
            try
            {
                var result = _source.GetContentStream(contentIdentifier);

                Logger.LogTrace($"End getting content: {contentIdentifier.Hash}");

                return result;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error getting content: {contentIdentifier.Hash}", e);
                throw;
            }
        }
    }
}
