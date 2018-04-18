using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Linq;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public class AmazonContent : IURLContent
    {
        private readonly string _bucketName;
        private readonly string _keyName;
        private readonly Amazon.S3.IAmazonS3 _amazonS3;
        private readonly IMultiPartUploadInfoFactory _multiPartUploadInfoFactory;
        private readonly IUploadIdentifierProvider _uploadIdentifierProvider;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IDownloadInfoFactory _downloadInfoFactory;
        private readonly IHttpPartUploadInfoFactory _httpPartUploadInfoFactory;

        private long? _size;

        public string SimpleName => _keyName.Split('/').Last();

        public AmazonContent(string bucketName, string keyName, long? size,
            Amazon.S3.IAmazonS3 amazonS3,
            IMultiPartUploadInfoFactory multiPartUploadInfoFactory,
            IUploadIdentifierProvider uploadIdentifierProvider,
            IContentNameProvider contentNameProvider,
            IDownloadInfoFactory downloadInfoFactory,
            IHttpPartUploadInfoFactory httpPartUploadInfoFactory)
        {
            _bucketName = bucketName;
            _keyName = keyName;
            _size = size;
            _amazonS3 = amazonS3;
            _multiPartUploadInfoFactory = multiPartUploadInfoFactory;
            _uploadIdentifierProvider = uploadIdentifierProvider;
            _contentNameProvider = contentNameProvider;
            _downloadInfoFactory = downloadInfoFactory;
            _httpPartUploadInfoFactory = httpPartUploadInfoFactory;
        }

        private string GetURL(int expirySeconds, Amazon.S3.HttpVerb httpVerb)
        {
            var request = 
                new Amazon.S3.Model.GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = _keyName,
                    Verb = httpVerb,
                    Expires = DateTime.UtcNow.AddSeconds(expirySeconds)
                };

            return _amazonS3.GetPreSignedURL(request);
        }

        public long GetSize()
        {
            if (!_size.HasValue)
            {
                var request =
                    new Amazon.S3.Model.GetObjectMetadataRequest
                    {
                        BucketName = _bucketName,
                        Key = _keyName
                    };

                _size = _amazonS3.GetObjectMetadataAsync(request).Result.ContentLength;
            }

            return _size.Value;
        }

        public IMultiPartUploadInfo GetMultiPartUploadInfo(int expirySeconds, long size)
        {
            var url = GetURL(expirySeconds, Amazon.S3.HttpVerb.PUT);
            var contentIdentifier = _contentNameProvider.GetContentIdentifier(_keyName.Split('/').Last());
            var uploadIdentifier = _uploadIdentifierProvider.GetUploadIdentifier(contentIdentifier, url);
            var httpPartUploadInfo = _httpPartUploadInfoFactory(null, url, null);

            return _multiPartUploadInfoFactory("PUT", 0, new[] { httpPartUploadInfo }, null, uploadIdentifier);
        }

        public IDownloadInfo GetDownloadInfo(int expirySeconds)
        {
            var url = GetURL(expirySeconds, Amazon.S3.HttpVerb.GET);
            var size = GetSize();

            return _downloadInfoFactory(url, size);
        }

        public void LoadFromStream(System.IO.Stream stream)
        {
            using (var fileTransferUtility = new Amazon.S3.Transfer.TransferUtility(_amazonS3))
            {
                fileTransferUtility.Upload(stream, _bucketName, _keyName);
            }
        }

        public System.IO.Stream GetReadStream()
        {
            var request = new Amazon.S3.Model.GetObjectRequest { BucketName = _bucketName, Key = _keyName };
            var response = _amazonS3.GetObjectAsync(request).Result;
            return response.ResponseStream;
        }
    }
}
