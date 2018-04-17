using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Base
{
    public class FileSystemContainer : IFileSystemContainer, ISystemContainer
    {
        private const int ChunkSize = 10000;

        private readonly string _root;
        private readonly string _dirName;
        private readonly IFileSystemContentFactory _fileSystemContentFactory;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IContentHashValidator _contentHashValidator;
        private readonly IFileSystemFullFileNameProvider _fileSystemFullFileNameProvider;
        private readonly IContentsMonthHashProvider _contentsMonthHashProvider;
        private readonly IHashProvider _hashProvider;

        public FileSystemContainer(string root, string dirName, 
            IFileSystemContentFactory fileSystemContentFactory,
            IContentNameProvider contentNameProvider,
            IContentIdentifierGenerator contentIdentifierGenerator,
            IContentHashValidator contentHashValidator,
            IFileSystemFullFileNameProvider fileSystemFullFileNameProvider,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IHashProvider hashProvider)
        {
            _root = root;
            _dirName = dirName;
            _fileSystemContentFactory = fileSystemContentFactory;
            _contentNameProvider = contentNameProvider;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _contentHashValidator = contentHashValidator;
            _fileSystemFullFileNameProvider = fileSystemFullFileNameProvider;
            _hashProvider = hashProvider;
            _contentsMonthHashProvider = contentsMonthHashProvider;
            _hashProvider = hashProvider;
        }

        private string GetContentFullFileName(string contentName)
        {
            return _fileSystemFullFileNameProvider.GetFullFileName(_root, _dirName, contentName);
        }

        private bool ContentExists(string contentName)
        {
            if (contentName == null)
                return false;

            var contentFullFileName = GetContentFullFileName(contentName);

            return System.IO.File.Exists(contentFullFileName);
        }

        public bool ContentExists(IContentIdentifier contentIdentifier)
        {
            var contentName = _contentNameProvider.GetContentName(contentIdentifier);
            return ContentExists(contentName);
        }

        public IContent CreateContent(IContentIdentifier contentIdentifier)
        {
            if (ContentExists(contentIdentifier))
                throw new UserException("Content already exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            return _fileSystemContentFactory(_root, _dirName, contentName);
        }

        public IContent GetContent(IContentIdentifier contentIdentifier)
        {
            if (!ContentExists(contentIdentifier))
                throw new UserException("Content does not exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            return GetContent(contentName);
        }

        public IContent GetContent(string contentName)
        {
            return _fileSystemContentFactory(_root, _dirName, contentName);
        }

        public IEnumerable<string> GetContentNames(string prefix)
        {
            string fullDirName;

            if (string.IsNullOrEmpty(prefix))
            {
                fullDirName = System.IO.Path.Combine(_root, _dirName);
            }
            else
            {
                var prefixParts = prefix.Split('/');
                var dirs = prefixParts.Reverse().Skip(1).Reverse();
                prefix = prefixParts.Last();

                var subDirs = System.IO.Path.Combine(dirs.ToArray());
                fullDirName = System.IO.Path.Combine(_root, _dirName, subDirs);
            }

            if (!System.IO.Directory.Exists(fullDirName))
            {
                return Enumerable.Empty<string>();
            }

            // optimize with FastDirectoryEnumerator http://www.codeproject.com/Articles/38959/A-Faster-Directory-Enumerator
            var fileNames = System.IO.Directory.EnumerateFiles(fullDirName, prefix + "*.*");

            return
                fileNames
                .Select(x => System.IO.Path.GetFileName(x));

        }

        public IEnumerable<IContent> GetContents(string prefix)
        {
            return GetContentNames(prefix).Select(x => GetContent(x));
        }

        public async Task<IContentIdentifier> CommitContentAsync(IContentIdentifier uncommittedContentIdentifier)
        {
            System.Diagnostics.Debug.Assert(uncommittedContentIdentifier.Uncommitted);

            var unncommittedContent = GetContent(uncommittedContentIdentifier);
            _contentHashValidator.ValidateHash(unncommittedContent, uncommittedContentIdentifier.Hash);

            var uncommittedContentName = _contentNameProvider.GetContentName(uncommittedContentIdentifier);
            var contentIdentifier = _contentIdentifierGenerator.GetCommittedContentIdentifier(uncommittedContentIdentifier);
            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            RenameContent(uncommittedContentName, contentName);

            return await Task.FromResult(contentIdentifier);
        }

        private void RenameContent(string oldContentName, string newContentName)
        {
            var oldContentFullFileName = GetContentFullFileName(oldContentName);
            var newContentFullFileName = GetContentFullFileName(newContentName);

            System.IO.File.Delete(newContentFullFileName);
            System.IO.File.Move(oldContentFullFileName, newContentFullFileName);
        }
    }
}
