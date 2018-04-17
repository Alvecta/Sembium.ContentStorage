using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Base
{
    public class FileSystemContentStorageHost : IFileSystemContentStorageHost
    {
        private const string StorageRootAppSettingName = "StorageRoot";

        private readonly string _storageRoot;
        private readonly IFileSystemContainerFactory _fileSystemContainerFactory;

        public FileSystemContentStorageHost(string storageRoot,
            IFileSystemContainerFactory fileSystemContainerFactory)
        {
            _storageRoot = storageRoot;
            _fileSystemContainerFactory = fileSystemContainerFactory;
        }

        public bool ContainerExists(string containerName)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(_storageRoot, containerName));
        }

        public IContainer CreateContainer(string containerName)
        {
            if (ContainerExists(containerName))
                throw new UserException("Container already exist");

            InternalCreateContainer(containerName);

            return _fileSystemContainerFactory(_storageRoot, containerName);
        }

        private void InternalCreateContainer(string containerName)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_storageRoot, containerName));
        }

        public IContainer GetContainer(string containerName, bool createIfNotExists = false)
        {
            if (!ContainerExists(containerName))
            {
                if (!createIfNotExists)
                {
                    throw new UserException("Container does not exist: " + containerName);
                }

                InternalCreateContainer(containerName);
            }

            return _fileSystemContainerFactory(_storageRoot, containerName);
        }

        public IEnumerable<string> GetContainerNames()
        {
            return
                System.IO.Directory.GetDirectories(_storageRoot)
                .Select(x => System.IO.Path.GetFileName(x))
                .OrderBy(x => x);
        }

        public IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken)
        {
            return null;
        }
    }
}
