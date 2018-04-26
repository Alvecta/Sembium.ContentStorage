using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemContainerProvider : IFileSystemContainerProvider
    {
        private readonly IFileSystemContentStorageHostFactory _fileSystemContentStorageHostFactory;

        public FileSystemContainerProvider(
            IFileSystemContentStorageHostFactory fileSystemContentStorageHostFactory)
        {
            _fileSystemContentStorageHostFactory = fileSystemContentStorageHostFactory;
        }

        public IContainer GetContainer(string dirName)
        {
            var parentDirName = System.IO.Path.GetDirectoryName(dirName);
            var dirShortName = System.IO.Path.GetFileName(dirName);

            var fileSystemContentStorageHost = _fileSystemContentStorageHostFactory(parentDirName);

            return
                (fileSystemContentStorageHost.ContainerExists(dirShortName)) ?
                fileSystemContentStorageHost.GetContainer(dirShortName) :
                fileSystemContentStorageHost.CreateContainer(dirShortName);
        }
    }
}
