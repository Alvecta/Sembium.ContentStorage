using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public delegate IFileStoredCompleteMomentConfig IFileStoredCompleteMomentConfigFactory(string fileName, int expiryMinutes);
}
