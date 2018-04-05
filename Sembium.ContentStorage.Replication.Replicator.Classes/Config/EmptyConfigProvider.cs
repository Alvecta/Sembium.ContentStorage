using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class EmptyConfigProvider : IConfigProvider
    {
        private readonly IConfigFactory _configFactory;

        public EmptyConfigProvider(IConfigFactory configFactory)
        {
            _configFactory = configFactory;
        }

        public IConfig GetConfig()
        {
            return _configFactory(new IRouteConfig[] { }, null);
        }

        public bool CanProvide()
        {
            return true;
        }
    }
}
