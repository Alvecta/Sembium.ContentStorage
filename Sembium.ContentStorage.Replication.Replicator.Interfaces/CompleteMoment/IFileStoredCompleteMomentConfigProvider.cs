using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public interface IFileStoredCompleteMomentConfigProvider
    {
        IFileStoredCompleteMomentConfig GetConfig();
    }
}
