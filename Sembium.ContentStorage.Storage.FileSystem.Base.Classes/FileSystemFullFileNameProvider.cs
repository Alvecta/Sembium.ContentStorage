using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Base
{
    public class FileSystemFullFileNameProvider : IFileSystemFullFileNameProvider
    {
        public string GetFullFileName(string root, string dirName, string fileName)
        {
            return System.IO.Path.Combine(root, dirName, fileName.Replace("/", @"\"));
        }
    }
}
