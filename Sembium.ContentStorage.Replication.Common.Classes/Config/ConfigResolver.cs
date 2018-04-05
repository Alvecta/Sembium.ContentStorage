using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Config
{
    public class ConfigResolver : IConfigResolver
    {
        private readonly IEnumerable<IConfigProvider> _providers;

        public ConfigResolver(IEnumerable<IConfigProvider> providers)
        {
            _providers = providers;
        }

        public IConfig GetConfig()
        {
            return _providers.Where(x => x.CanProvide()).Select(x => x.GetConfig()).FirstOrDefault();
        }
    }
}
