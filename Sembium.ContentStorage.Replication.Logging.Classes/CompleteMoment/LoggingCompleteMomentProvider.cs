using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.CompleteMoment
{
    public class LoggingCompleteMomentProvider : ICompleteMomentProvider
    {
        private readonly ICompleteMomentProvider _completeMomentProvider;
        private readonly ILogger _logger;

        public LoggingCompleteMomentProvider(ICompleteMomentProvider completeMomentProvider, ILogger logger)
        {
            _completeMomentProvider = completeMomentProvider;
            _logger = logger;
        }

        public DateTimeOffset GetCompleteMoment(string sourceID, string destinationID)
        {
            _logger.LogTrace($"Start getting complete moment for source {sourceID} and destination {destinationID}");

            var result = _completeMomentProvider.GetCompleteMoment(sourceID, destinationID);

            _logger.LogTrace($"End getting complete moment for source {sourceID} and destination {destinationID}");

            return result;
        }

        public void SetCompleteMoment(string sourceID, string destinationID, DateTimeOffset moment)
        {
            _completeMomentProvider.SetCompleteMoment(sourceID, destinationID, moment);
        }

        public void Finish()
        {
            _completeMomentProvider.Finish();
        }
    }
}
