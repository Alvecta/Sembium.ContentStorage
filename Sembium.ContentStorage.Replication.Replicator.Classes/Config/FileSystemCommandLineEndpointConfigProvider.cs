using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.FileSystem.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class FileSystemCommandLineEndpointConfigProvider : ICommandLineEndpointConfigProvider
    {
        private readonly IFileSystemEndpointConfigFactory _fileSystemEndpointConfigFactory;

        public FileSystemCommandLineEndpointConfigProvider(IFileSystemEndpointConfigFactory FileSystemEndpointConfigFactory)
        {
            _fileSystemEndpointConfigFactory = FileSystemEndpointConfigFactory;
        }

        public IEndpointConfig GetConfig(string[] args)
        {
            return _fileSystemEndpointConfigFactory(args[1]);
        }

        public bool CanProvideConfig(string[] args)
        {
            return (args != null) && (args.Count() == 2) && string.Equals(args[0], "dir", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
