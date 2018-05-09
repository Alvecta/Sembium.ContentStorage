using Sembium.ContentStorage.Client;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.Tools;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Destination
{
    public class ContentStorageDestination : ContentStorageEndpoint, IContentStorageDestination
    {
        private readonly IContentStorageUploader _contentStorageUploader;

        public ContentStorageDestination(string containerName, string authenticationToken,
            string contentStorageServiceURL,
            IContentStorageServiceURLProvider contentStorageServiceURLProvider,
            IContentIdentifierSerializer contentIdentifierSerializer,
            ISerializer serializer,
            IContentStorageUploader contentStorageUploader)
            : base(containerName, authenticationToken, contentStorageServiceURL, contentStorageServiceURLProvider, contentIdentifierSerializer, serializer)
        {
            _contentStorageUploader = contentStorageUploader;
        }

        public void PutContent(IContentIdentifier contentIdentifier, ISource source)
        {
            var contentStorageSource = source as IContentStorageSource;

            if (contentStorageSource != null)
            {
                var downloadInfo = contentStorageSource.GetDownloadInfo(contentIdentifier);
                var urlContentUploadInfo = GetUrlContentUploadInfo(contentIdentifier, downloadInfo);
                if (urlContentUploadInfo != null)
                {
                    UploadUrlContent(urlContentUploadInfo);
                    return;
                }
            }

            var contentStream = source.GetContentStream(contentIdentifier);

            _contentStorageUploader.UploadContent(
                contentStream.Stream,
                ContentStorageServiceURL,
                ContainerName,
                contentIdentifier,
                contentStream.Size,
                AuthenticationToken
            );
        }

        private void UploadUrlContent(IHttpRequestInfo urlContentUploadInfo)
        {
            using (var httpClient = GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(urlContentUploadInfo.Method), urlContentUploadInfo.URL))
                {
                    request.SetHeaders(urlContentUploadInfo.Headers);
                    httpClient.CheckedSendAsync(request).Wait();
                }
            }
        }

        private IHttpRequestInfo GetUrlContentUploadInfo(IContentIdentifier contentIdentifier, IDownloadInfo downloadInfo)
        {
            var contentID = ContentIdentifierSerializer.Serialize(contentIdentifier);

            var urlForGetUrlContentUploadInfo =
                    ContentStorageServiceURLProvider.GetURLForGetUrlContentUploadInfo(
                        ContentStorageServiceURL,
                        ContainerName,
                        contentID,
                        downloadInfo.Size,
                        AuthenticationToken);

            using (var httpClient = GetHttpClient())
            {
                var headers =
                        new[] {
                            new KeyValuePair<string, string>("ContentUrl", downloadInfo.Url),
                            new KeyValuePair<string, string>("ContentStorageServiceUrl", ContentStorageServiceURL)
                        };

                var stringResult = httpClient.CheckedGetStringAsync(urlForGetUrlContentUploadInfo, headers).Result;

                return Serializer.Deserialize<HttpRequestInfo>(stringResult);
            }
        }
    }
}
