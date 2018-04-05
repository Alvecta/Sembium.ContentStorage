using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Main
{
    public interface IReplicationWorker
    {
        void Run(IConfig config);
    }
}
