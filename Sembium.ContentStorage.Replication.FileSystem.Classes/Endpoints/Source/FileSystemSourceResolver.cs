using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.FileSystem.Config;
using Sembium.ContentStorage.Storage.FileSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Source
{
    public class FileSystemSourceResolver : ISourceResolver
    {
        private readonly IFileSystemContainerProvider _fileSystemContainerProvider;
        private readonly IFileSystemSourceFactory _fileSystemSourceFactory;

        public FileSystemSourceResolver(
            IFileSystemContainerProvider fileSystemContainerProvider,
            IFileSystemSourceFactory fileSystemSourceFactory)
        {
            _fileSystemContainerProvider = fileSystemContainerProvider;
            _fileSystemSourceFactory = fileSystemSourceFactory;
        }

        public bool CanResolve(IEndpointConfig config)
        {
            return (config is IFileSystemEndpointConfig);
        }

        public ISource GetSource(IEndpointConfig config)
        {
            if (!CanResolve(config))
                throw new ArgumentException("Missing source config");

            var cfg = config as IFileSystemEndpointConfig;

            var fileSystemContainer = _fileSystemContainerProvider.GetContainer(cfg.DirectoryName);

            return  _fileSystemSourceFactory(cfg.DirectoryName, fileSystemContainer);
        }
    }
}
