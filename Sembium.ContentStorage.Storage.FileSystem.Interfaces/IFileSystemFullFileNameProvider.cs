using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public interface IFileSystemFullFileNameProvider
    {
        string GetFullFileName(string root, string dirName, string fileName);
    }
}
