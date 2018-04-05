using Newtonsoft.Json;
using Sembium.ContentStorage.Storage.Common;
using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class MultiPartUploadInfo : IMultiPartUploadInfo
    {
        public string HttpMethod { get; private set; }
        public long PartSize { get; private set; }

        [JsonConverter(typeof(ConcreteJsonConverter<IEnumerable<HttpPartUploadInfo>>))]
        public IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; private set; }

        public string MultiPartUploadResultHeaderName { get; private set; }
        public IUploadIdentifier UploadIdentifier { get; private set; }

        public MultiPartUploadInfo(string httpMethod, long partSize, IEnumerable<IHttpPartUploadInfo> partUploadInfos, string multiPartUploadResultHeaderName, IUploadIdentifier uploadIdentifier)
        {
            HttpMethod = httpMethod;
            PartSize = partSize;
            PartUploadInfos = partUploadInfos;
            MultiPartUploadResultHeaderName = multiPartUploadResultHeaderName;
            UploadIdentifier = uploadIdentifier;
        }
    }
}
