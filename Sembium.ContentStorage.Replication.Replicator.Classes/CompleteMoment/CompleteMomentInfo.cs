using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public class CompleteMomentInfo : ICompleteMomentInfo
    {
        public string SourceID { get; }
        public string DestinationID { get; }
        public DateTimeOffset Moment { get; }
        public DateTimeOffset OriginMoment { get; }

        private CompleteMomentInfo()
        {
            // do nothing
        }

        public CompleteMomentInfo(string sourceID, string destinationID, DateTimeOffset moment, DateTimeOffset originMoment)
        {
            SourceID = sourceID;
            DestinationID = destinationID;
            Moment = moment;
            OriginMoment = originMoment;
        }
    }
}
