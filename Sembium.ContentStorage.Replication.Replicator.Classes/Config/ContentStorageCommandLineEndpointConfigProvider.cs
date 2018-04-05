using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.ContentStorage.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class ContentStorageCommandLineEndpointConfigProvider : ICommandLineEndpointConfigProvider
    {
        private readonly IContentStorageEndpointConfigFactory _contentStorageEndpointConfigFactory;

        public ContentStorageCommandLineEndpointConfigProvider(IContentStorageEndpointConfigFactory contentStorageEndpointConfigFactory)
        {
            _contentStorageEndpointConfigFactory = contentStorageEndpointConfigFactory;
        }

        public IEndpointConfig GetConfig(string[] args)
        {
            return _contentStorageEndpointConfigFactory(args[1], args[2]);
        }

        public bool CanProvideConfig(string[] args)
        {
            return (args != null) && (args.Count() == 3) && string.Equals(args[0], "cs", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
