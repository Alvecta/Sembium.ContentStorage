using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public interface ICompleteMomentProvider
    {
        DateTimeOffset GetCompleteMoment(string sourceID, string destinationID);
        void SetCompleteMoment(string sourceID, string destinationID, DateTimeOffset moment);
        void Finish();
    }
}
