using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Base
{
    public class FileSystemContent : IContent
    {
        private readonly IFileSystemFullFileNameProvider _fileSystemFullFileNameProvider;

        protected string Root { get; private set; }
        protected string DirName { get; private set; }
        protected string FileName { get; private set; }

        private string FullFileName 
        { 
            get
            {
                return _fileSystemFullFileNameProvider.GetFullFileName(Root, DirName, FileName);
            }
        }

        public FileSystemContent(string root, string dirName, string fileName, 
            IFileSystemFullFileNameProvider fileSystemFullFileNameProvider)
        {
            Root = root;
            DirName = dirName;
            FileName = fileName;
            _fileSystemFullFileNameProvider = fileSystemFullFileNameProvider;
        }

        public void LoadFromStream(System.IO.Stream stream)
        {
            using (var fileStream = System.IO.File.Create(FullFileName))
            {
                stream.CopyTo(fileStream);
            }
        }

        public System.IO.Stream GetReadStream()
        {
            return System.IO.File.OpenRead(FullFileName);
        }

        public  long GetSize()
        {
            return new System.IO.FileInfo(FullFileName).Length;
        }
    }
}
