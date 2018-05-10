using Sembium.ContentStorage.Client;
using Sembium.ContentStorage.Misc;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.Tools;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source
{
    public class ContentStorageSource : ContentStorageEndpoint, IContentStorageSource  // todo: caching decorator for GetDownloadInfo
    {
        private readonly IContentStreamFactory _contentStreamFactory;

        public ContentStorageSource(string containerName, string authenticationToken,
            string contentStorageServiceURL,
            IContentStorageServiceURLProvider contentStorageServiceURLProvider,
            IContentIdentifierSerializer contentIdentifierSerializer,
            ISerializer serializer,
            IContentStreamFactory contentStreamFactory)
            : base(containerName, authenticationToken, contentStorageServiceURL, contentStorageServiceURLProvider, contentIdentifierSerializer, serializer)
        {
            _contentStreamFactory = contentStreamFactory;
        }

        public IContentStream GetContentStream(IContentIdentifier contentIdentifier)
        {
            var downloadInfo = GetDownloadInfo(contentIdentifier);

            using (var httpClient = GetHttpClient())
            {
                return
                    _contentStreamFactory(
                        downloadInfo.Size,
                        httpClient.CheckedGetStreamAsync(downloadInfo.Url).Result
                    );
            }
        }

        public IDownloadInfo GetDownloadInfo(IContentIdentifier contentIdentifier)
        {
            var contentID = ContentIdentifierSerializer.Serialize(contentIdentifier);

            var requestURL = ContentStorageServiceURLProvider.GetURLForGetContentDownloadInfo(ContentStorageServiceURL, ContainerName, contentID, AuthenticationToken);

            using (var httpClient = GetHttpClient())
            {
                var downloadinfoJson = httpClient.CheckedGetStringAsync(requestURL).Result;

                return Serializer.Deserialize<IDownloadInfo>(downloadinfoJson);
            }
        }
    }
}
