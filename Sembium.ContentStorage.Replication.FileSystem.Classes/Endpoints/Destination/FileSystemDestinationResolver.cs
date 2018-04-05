using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.FileSystem.Config;
using Sembium.ContentStorage.Storage.FileSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Destination
{
    public class FileSystemDestinationResolver : IDestinationResolver
    {
        private readonly IFileSystemContainerProvider _fileSystemContainerProvider;
        private readonly IFileSystemDestinationFactory _fileSystemDestinationFactory;

        public FileSystemDestinationResolver(
            IFileSystemContainerProvider fileSystemContainerProvider,
            IFileSystemDestinationFactory fileSystemDestinationFactory)
        {
            _fileSystemContainerProvider = fileSystemContainerProvider;
            _fileSystemDestinationFactory = fileSystemDestinationFactory;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return (config is IFileSystemEndpointConfig);
        }

        public IDestination GetDestination(IEndpointConfig config)
        {
            if (!CanResolve(config))
                throw new ArgumentException("Missing destination config");

            var cfg = config as IFileSystemEndpointConfig;

            var fileSystemContainer = _fileSystemContainerProvider.GetContainer(cfg.DirectoryName);

            return _fileSystemDestinationFactory(cfg.DirectoryName, fileSystemContainer);
        }
    }
}
