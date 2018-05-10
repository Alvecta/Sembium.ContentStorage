using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class MultiPartUploadInfo : IMultiPartUploadInfo
    {
        public string HttpMethod { get; }
        public long PartSize { get; }

        public IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; }

        public string MultiPartUploadResultHeaderName { get; }
        public IUploadIdentifier UploadIdentifier { get; }

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
