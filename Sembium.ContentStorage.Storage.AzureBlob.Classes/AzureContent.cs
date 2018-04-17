using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContent : IURLContent
    {
        private const long MaxSinglePartUploadSize = 64 * 1024 * 1024;
        private const long PartUploadSize = 4 * 1024 * 1024;

        private readonly Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob _delegateContent;
        private readonly IMultiPartUploadInfoFactory _multiPartUploadInfoFactory;
        private readonly IUploadIdentifierProvider _uploadIdentifierProvider;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IDownloadInfoFactory _downloadInfoFactory;
        private readonly IHttpPartUploadInfoFactory _httpPartUploadInfoFactory;

        public string Name => _delegateContent.Name;

        public AzureContent(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob delegateContent,
            IMultiPartUploadInfoFactory multiPartUploadInfoFactory,
            IUploadIdentifierProvider uploadIdentifierProvider,
            IContentNameProvider contentNameProvider,
            IDownloadInfoFactory downloadInfoFactory,
            IHttpPartUploadInfoFactory httpPartUploadInfoFactory)
        {
            _delegateContent = delegateContent;
            _multiPartUploadInfoFactory = multiPartUploadInfoFactory;
            _uploadIdentifierProvider = uploadIdentifierProvider;
            _contentNameProvider = contentNameProvider;
            _downloadInfoFactory = downloadInfoFactory;
            _httpPartUploadInfoFactory = httpPartUploadInfoFactory;
        }

        // source: http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-2/
        private string GetURL(int expirySeconds, Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions permissions)
        {
            var sasConstraints = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.Now.AddMinutes(-1);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.Now.AddSeconds(expirySeconds);
            sasConstraints.Permissions = permissions;

            var sasToken = _delegateContent.GetSharedAccessSignature(sasConstraints);

            return _delegateContent.Uri + sasToken;
        }

        public IMultiPartUploadInfo GetMultiPartUploadInfo(int expirySeconds, long size)
        {
            var url = GetURL(expirySeconds, Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Write);
            var contentIdentifier = _contentNameProvider.GetContentIdentifier(_delegateContent.Name);

            if (size <= MaxSinglePartUploadSize)
            {
                var uploadIdentifier = _uploadIdentifierProvider.GetUploadIdentifier(contentIdentifier, null);
                var httpPartUploadInfo = GetHttpPartUploadInfo(null, url);
                return _multiPartUploadInfoFactory("PUT", 0, new[] { httpPartUploadInfo }, null, uploadIdentifier);
            }
            else
            {
                var uploadIdentifier = _uploadIdentifierProvider.GetUploadIdentifier(contentIdentifier, url);
                var httpPartUploadInfos = GetHttpPartUploadInfos(url,  size);
                return _multiPartUploadInfoFactory("PUT", PartUploadSize, httpPartUploadInfos, null, uploadIdentifier);
            }
        }

        private IEnumerable<IHttpPartUploadInfo> GetHttpPartUploadInfos(string baseUrl, long size)
        {
            var partCount = (size / PartUploadSize) + Math.Sign(size / PartUploadSize);

            return Enumerable.Range(0, (int)partCount).Select(x => GetAzurePartUploadInfo(x, baseUrl));
        }

        private IHttpPartUploadInfo GetAzurePartUploadInfo(int partNo, string baseUrl)
        {
            var identifier = GetPartIdentifier(partNo);
            var url = $"{baseUrl}&comp=block&blockid={identifier}";
            return GetHttpPartUploadInfo(identifier, url);
        }

        private static string GetPartIdentifier(int partNo)
        {
            var x = "Block" + partNo.ToString().PadLeft(10, '0');
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(x));
        }

        private IHttpPartUploadInfo GetHttpPartUploadInfo(string identifier, string url)
        {
            return _httpPartUploadInfoFactory(identifier, url, new[] { new KeyValuePair<string, string>("x-ms-blob-type", "BlockBlob") });
        }

        public IDownloadInfo GetDownloadInfo(int expirySeconds)
        {
            var url = GetURL(expirySeconds, Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read);
            var size = GetSize();

            return _downloadInfoFactory(url, size);
        }

        public long GetSize()
        {
            if (_delegateContent.Properties.Length < 0)
            {
                _delegateContent.FetchAttributesAsync().Wait();
            }

            return _delegateContent.Properties.Length;
        }

        public void LoadFromStream(System.IO.Stream stream)
        {
            _delegateContent.UploadFromStreamAsync(stream).Wait();
        }

        public System.IO.Stream GetReadStream()
        {
            return _delegateContent.OpenReadAsync().Result;
        }
    }
}
