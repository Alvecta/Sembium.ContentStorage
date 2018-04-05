using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public class CompleteMomentInfo : ICompleteMomentInfo
    {
        public string SourceID { get; private set; }
        public string DestinationID { get; private set; }
        public DateTimeOffset Moment { get; private set; }
        public DateTimeOffset OriginMoment { get; private set; }

        public CompleteMomentInfo(string sourceID, string destinationID, DateTimeOffset moment, DateTimeOffset originMoment)
        {
            SourceID = sourceID;
            DestinationID = destinationID;
            Moment = moment;
            OriginMoment = originMoment;
        }
    }
}
