using Sembium.ContentStorage.Client;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Tools;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common
{
    public abstract class ContentStorageEndpoint : IEndpoint
    {
        private bool _sslDisabled;

        protected string ContainerName { get; private set; }
        protected string AuthenticationToken { get; private set; }
        protected string ContentStorageServiceURL { get; private set; }
        protected IContentStorageServiceURLProvider ContentStorageServiceURLProvider { get; private set; }
        protected IContentIdentifierSerializer ContentIdentifierSerializer { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public ContentStorageEndpoint(string containerName, string authenticationToken, 
            string contentStorageServiceURL,
            IContentStorageServiceURLProvider contentStorageServiceURLProvider,
            IContentIdentifierSerializer contentIdentifierSerializer,
            ISerializer serializer)
        {
            ContainerName = containerName;
            AuthenticationToken = authenticationToken;

            ContentStorageServiceURL = contentStorageServiceURL;
            ContentStorageServiceURLProvider = contentStorageServiceURLProvider;
            ContentIdentifierSerializer = contentIdentifierSerializer;
            Serializer = serializer;
        }

        public string ID
        {
            get { return string.Format(@"CS:\{0}@{1}", ContainerName, ContentStorageServiceURL); }
        }

        protected System.Net.Http.HttpClient GetHttpClient()
        {
            if (!_sslDisabled)
            {
                _sslDisabled = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };  
            }

            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(5);

            return httpClient;
        }

        public async Task<IEnumerable<IContentIdentifier>> GetContentIdentifiersAsync(DateTimeOffset afterMoment)
        {
            return await Task.FromResult(GetContentIdentifiers(afterMoment).ToList());
        }

        private IEnumerable<IContentIdentifier> GetContentIdentifiers(DateTimeOffset afterMoment)
        {
            afterMoment = new[] { afterMoment, new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.FromTicks(0)) }.Max();

            using (var httpClient = GetHttpClient())
            {
                IContentIdentifier lastContentIdentifier = null;

                while (true)
                {
                    var afterContentID = (lastContentIdentifier == null) ? null : ContentIdentifierSerializer.Serialize(lastContentIdentifier);
                    var moment = (lastContentIdentifier == null) ? (DateTimeOffset?)afterMoment : null;

                    var requestURL = ContentStorageServiceURLProvider.GetURLForGetContentIdentifiers(ContentStorageServiceURL, ContainerName, moment, null, afterContentID, AuthenticationToken);

                    using (var stream = httpClient.CheckedGetStreamAsync(requestURL).Result)
                    {
                        var contentIdentifiers =
                                stream
                                .ReadAllLines(Encoding.UTF8)
                                .Select(x => ContentIdentifierSerializer.Deserialize(x))
                                .ToList();

                        var hasContentIdentifiers = false;
                        foreach (var contentIdentifier in contentIdentifiers)
                        {
                            yield return contentIdentifier;

                            lastContentIdentifier = contentIdentifier;
                            hasContentIdentifiers = true;
                        }

                        if (!hasContentIdentifiers)
                        {
                            break;
                        }

                        // todo: remove this when deployed everywhere
                        var supportsContinuation = requestURL.StartsWith("https");

                        if (!supportsContinuation)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public async Task<string> GetContentsHashAsync(DateTimeOffset beforeMoment)
        {
            var moment = new[] { beforeMoment, new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.FromTicks(0)) }.Max();

            var requestURL = ContentStorageServiceURLProvider.GetURLForGetContentsHash(ContentStorageServiceURL, ContainerName, moment, AuthenticationToken);

            using (var httpClient = GetHttpClient())
            {
                var result = await httpClient.CheckedGetStringAsync(requestURL);

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Trim('"');
                }

                return result;
            }
        }
    }
}
