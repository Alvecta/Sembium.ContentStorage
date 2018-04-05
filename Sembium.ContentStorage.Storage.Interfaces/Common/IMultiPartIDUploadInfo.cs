using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Common
{
    public interface IMultiPartIDUploadInfo
    {
        string HttpMethod { get; }
        long PartSize { get; }
        IEnumerable<IHttpPartUploadInfo> PartUploadInfos { get; }
        string MultiPartUploadResultHeaderName { get; }
        string UploadID { get; }
    }
}
