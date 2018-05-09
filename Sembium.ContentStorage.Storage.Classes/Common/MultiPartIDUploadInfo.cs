using Sembium.ContentStorage.Storage.HostingResults;
using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.Common
{
    public class MultiPartIDUploadInfo : IMultiPartIDUploadInfo
    {
        public string HttpMethod { get; private set; }
        public long PartSize { get; private set; }

        public IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; private set; }

        public string MultiPartUploadResultHeaderName { get; private set; }
        public string UploadID { get; private set; }

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
