using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public interface IMultiPartUploadInfo
    {
        string HttpMethod { get; }
        long PartSize { get; }
        IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; }
        string MultiPartUploadResultHeaderName { get; }
        IUploadIdentifier UploadIdentifier { get; }
    }
}
