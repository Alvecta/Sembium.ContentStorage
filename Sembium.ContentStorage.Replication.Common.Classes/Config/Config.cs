using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class Config : IConfig
    {
        public IEnumerable<IRouteConfig> RouteConfigs { get; }
        public string LogFileName { get; }

        public Config(IEnumerable<IRouteConfig> routeConfigs, string logFileName)
        {
            RouteConfigs = routeConfigs;
            LogFileName = logFileName;
        }
    }
}
