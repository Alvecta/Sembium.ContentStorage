using Sembium.ContentStorage.Storage.HostingResults;
using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.Common
{
    public class MultiPartIDUploadInfo : IMultiPartIDUploadInfo
    {
        public string HttpMethod { get; }
        public long PartSize { get; }

        public IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; }

        public string MultiPartUploadResultHeaderName { get; }
        public string UploadID { get; }

        private MultiPartIDUploadInfo()
        {
            // do nothing
        }

        public MultiPartIDUploadInfo(string httpMethod, long partSize, IEnumerable<IHttpPartUploadInfo> partUploadInfos, string multiPartUploadResultHeaderName, string uploadID)
        {
            HttpMethod = httpMethod;
            PartSize = partSize;
            PartUploadInfos = partUploadInfos;
            MultiPartUploadResultHeaderName = multiPartUploadResultHeaderName;
            UploadID = uploadID;
        }
    }
}
