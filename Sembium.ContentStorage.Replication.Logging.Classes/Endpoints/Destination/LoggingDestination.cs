using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Destination
{
    public class LoggingDestination : LoggingEndpoint, ILoggingDestination
    {
        private readonly IDestination _destination;

        public LoggingDestination(IDestination destination, ILogger logger)
            : base(destination, logger)
        {
            _destination = destination;
        }

        public void PutContent(IContentIdentifier contentIdentifier, ISource source)
        {
            Logger.LogTrace($"Start putting content: {contentIdentifier.Hash}");
            try
            {
                _destination.PutContent(contentIdentifier, source);

                Logger.LogTrace($"End putting content: {contentIdentifier.Hash}");
            }
            catch (Exception e)
            {
                Logger.LogError($"Error putting content: {contentIdentifier.Hash}" + Environment.NewLine + e.GetAggregateMessages(), e);
                throw;
            }
        }
    }
}
