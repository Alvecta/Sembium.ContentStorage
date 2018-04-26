using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemContent : IContent, ISystemContent
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

        public string SimpleName => FileName;

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
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FullFileName));

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

        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => System.IO.File.Delete(FullFileName));
        }
    }
}
