using Amazon.S3;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public class AmazonContentStorageHost : IContentStorageHost
    {
        private const string AmazonS3BucketNameSettingName = "AmazonS3BucketName";
        private const string UploaderServiceUrlSettingName = "UploaderServiceUrl";
        private const string UploaderAwsApiGatewayKeySettingName = "UploaderAwsApiGatewayKey";

        private readonly IAmazonContainerFactory _amazonContainerFactory;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IAmazonS3 _amazonS3;
        private readonly IHttpRequestInfoFactory _httpRequestInfoFactory;
        private string _contentStorageBucketName;

        public string ContentStorageBucketName
        {
            get
            {
                if (string.IsNullOrEmpty(_contentStorageBucketName))
                {
                    _contentStorageBucketName = _configurationSettings.GetAppSetting(AmazonS3BucketNameSettingName);
                }

                return _contentStorageBucketName;
            }
        }

        public AmazonContentStorageHost(
            IAmazonContainerFactory amazonContainerFactory,
            IConfigurationSettings configurationSettings,
            IAmazonS3 amazonS3,
            IHttpRequestInfoFactory httpRequestInfoFactory)
        {
            _amazonContainerFactory = amazonContainerFactory;
            _configurationSettings = configurationSettings;
            _amazonS3 = amazonS3;
            _httpRequestInfoFactory = httpRequestInfoFactory;
        }

        public bool ContainerExists(string containerName)
        {
            return _amazonS3.GetObjectExists(ContentStorageBucketName, containerName + "/");
        }

        public IContainer CreateContainer(string containerName)
        {
            if (ContainerExists(containerName))
                throw new UserException("Container already exist");

            using (var emptyStream = new MemoryStream(new byte[0]))
            {
                var folderRequest = new Amazon.S3.Model.PutObjectRequest()
                {
                    BucketName = ContentStorageBucketName,
                    Key = containerName + "/",
                    InputStream = emptyStream
                };

                var folderResponse = _amazonS3.PutObjectAsync(folderRequest).Result;
            }

            return _amazonContainerFactory(ContentStorageBucketName, containerName);
        }

        public IContainer GetContainer(string containerName)
        {
            if (!ContainerExists(containerName))
                throw new UserException("Container does not exist: " + containerName);

            return _amazonContainerFactory(ContentStorageBucketName, containerName);
        }

        public IEnumerable<string> GetContainerNames()
        {
            const char delimiter = '/';

            var request = new Amazon.S3.Model.ListObjectsRequest { BucketName = ContentStorageBucketName, Delimiter = delimiter.ToString() };

            while (true)
            {
                var response = _amazonS3.ListObjectsAsync(request).Result;

                foreach (var commonPrefix in response.CommonPrefixes)
                {
                    yield return commonPrefix.TrimEnd(delimiter);
                }

                if (!response.IsTruncated)
                    break;

                request.Marker = response.NextMarker;
            }
        }

        public IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken)
        {
            var uploaderServiceUrl = _configurationSettings.GetAppSetting(UploaderServiceUrlSettingName);

            if (string.IsNullOrEmpty(uploaderServiceUrl))
            {
                return null;
            }

            var uploaderAwsApiGatewayKey = _configurationSettings.GetAppSetting(UploaderAwsApiGatewayKeySettingName);

            var uploadMethodUrl = uploaderServiceUrl.TrimEnd('/') + "/upload";

            var headers = 
                    new[] {
                        new KeyValuePair<string, string>("Uploader-ContentUrl", contentUrl),
                        new KeyValuePair<string, string>("Uploader-ContentStorageServiceUrl", contentStorageServiceUrl),
                        new KeyValuePair<string, string>("Uploader-ContainerName", containerName),
                        new KeyValuePair<string, string>("Uploader-ContentID", contentID),
                        new KeyValuePair<string, string>("Uploader-Size", size.ToString()),
                        new KeyValuePair<string, string>("Uploader-AuthenticationToken", authenticationToken),
                        new KeyValuePair<string, string>("x-api-key", uploaderAwsApiGatewayKey)
                    };

            return _httpRequestInfoFactory(uploadMethodUrl, "POST", headers);
        }
    }
}
