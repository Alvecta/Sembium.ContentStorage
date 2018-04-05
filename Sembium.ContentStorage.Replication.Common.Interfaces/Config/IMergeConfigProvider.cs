using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public interface IMergeConfigProvider
    {
        IConfig GetConfig(IConfig config1, IConfig config2);
    }
}
