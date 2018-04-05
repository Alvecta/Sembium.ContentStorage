using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public delegate ICompleteMomentInfo ICompleteMomentInfoFactory(string sourceID, string destinationID, DateTimeOffset moment, DateTimeOffset originMoment);
}
