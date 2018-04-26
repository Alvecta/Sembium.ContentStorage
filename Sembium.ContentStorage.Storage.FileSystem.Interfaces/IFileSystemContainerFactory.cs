using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public delegate IFileSystemContainer IFileSystemContainerFactory(string root, string dirName);
}
