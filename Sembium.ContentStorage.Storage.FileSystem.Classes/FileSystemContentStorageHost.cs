using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemContentStorageHost : Sembium.ContentStorage.Storage.FileSystem.Base.FileSystemContentStorageHost
    {
        public FileSystemContentStorageHost(
            Sembium.ContentStorage.Storage.FileSystem.Base.IFileSystemContainerFactory fileSystemContainerFactory,
            IFileSystemStorageRootProvider fileSystemStorageRootProvider)
            : base(fileSystemStorageRootProvider.GetRoot(), fileSystemContainerFactory)
        {
        }
    }
}
