using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Storage.Tools;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemURLContent : FileSystemContent, IURLContent
    {
        private readonly IMultiPartUploadInfoFactory _multiPartUploadInfoFactory;
        private readonly IUploadIdentifierProvider _uploadIdentifierProvider;
        private readonly ITransferServiceProvider _transferServiceProvider;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IDownloadInfoFactory _downloadInfoFactory;
        private readonly IHttpPartUploadInfoFactory _httpPartUploadInfoFactory;

        public FileSystemURLContent(string root, string dirName, string fileName,
            IMultiPartUploadInfoFactory multiPartUploadInfoFactory,
            IUploadIdentifierProvider uploadIdentifierProvider,
            ITransferServiceProvider transferServiceProvider,
            IContentNameProvider contentNameProvider,
            IFileSystemFullFileNameProvider fileSystemFullFileNameProvider,
            IDownloadInfoFactory downloadInfoFactory,
            IHttpPartUploadInfoFactory httpPartUploadInfoFactory)
            : base(root, dirName, fileName, fileSystemFullFileNameProvider)
        {
            _multiPartUploadInfoFactory = multiPartUploadInfoFactory;
            _uploadIdentifierProvider = uploadIdentifierProvider;
            _transferServiceProvider = transferServiceProvider;
            _contentNameProvider = contentNameProvider;
            _downloadInfoFactory = downloadInfoFactory;
            _httpPartUploadInfoFactory = httpPartUploadInfoFactory;
        }

        public IMultiPartUploadInfo GetMultiPartUploadInfo(int expirySeconds, long size)
        {
            var url = _transferServiceProvider.GetURL(DirName, FileName, "upload", expirySeconds);
            var contentIdentifier = _contentNameProvider.GetContentIdentifier(FileName);
            var uploadIdentifier = _uploadIdentifierProvider.GetUploadIdentifier(contentIdentifier, null);
            var httpPartUploadInfo = _httpPartUploadInfoFactory(null, url, null);

            return _multiPartUploadInfoFactory("PUT/FORMFILE", 0, new[] { httpPartUploadInfo }, null, uploadIdentifier);
        }

        public IDownloadInfo GetDownloadInfo(int expirySeconds)
        {
            var url = _transferServiceProvider.GetURL(DirName, FileName, "download", expirySeconds);
            var size = GetSize();

            return _downloadInfoFactory(url, size);
        }
    }
}
