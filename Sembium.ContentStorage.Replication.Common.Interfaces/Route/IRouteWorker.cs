using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public interface IRouteWorker
    {
        void RunRoute(IRouteConfig routeConfig);
        void HashCheckRoute(IRouteConfig routeConfig);
    }
}
