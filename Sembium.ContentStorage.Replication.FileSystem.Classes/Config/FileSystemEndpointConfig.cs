using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Config
{
    public class FileSystemEndpointConfig : IFileSystemEndpointConfig
    {
        public string DirectoryName { get; private set; }

        public FileSystemEndpointConfig(string directoryName)
        {
            DirectoryName = directoryName;
        }
    }
}
