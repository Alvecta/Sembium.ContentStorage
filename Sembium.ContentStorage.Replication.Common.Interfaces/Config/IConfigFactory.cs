using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public delegate IConfig IConfigFactory(IEnumerable<IRouteConfig> routeConfigs, string logFileName);
}
