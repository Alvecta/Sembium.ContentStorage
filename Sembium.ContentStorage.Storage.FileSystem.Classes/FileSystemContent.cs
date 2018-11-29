using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemContent : IContent, ISystemContent
    {
        private readonly IFileSystemFullFileNameProvider _fileSystemFullFileNameProvider;

        protected string Root { get; }
        protected string DirName { get; }
        protected string FileName { get; }

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

        public System.IO.Stream GetContents(bool emptyIfMissing)
        {
            try
            {
                var readStream = GetReadStream();

                var result = new System.IO.MemoryStream();
                readStream.CopyTo(result);
                result.Position = 0;

                return result;
            }
            catch (FileNotFoundException)
            {
                if (emptyIfMissing)
                {
                    return new System.IO.MemoryStream();
                }

                throw;
            }
        }

        public long GetSize()
        {
            return new System.IO.FileInfo(FullFileName).Length;
        }

        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => System.IO.File.Delete(FullFileName));
        }
    }
}
