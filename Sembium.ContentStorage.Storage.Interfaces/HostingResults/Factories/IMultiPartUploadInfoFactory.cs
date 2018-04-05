using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults.Factories
{
    public delegate IMultiPartUploadInfo IMultiPartUploadInfoFactory(string httpMethod, long partSize, IEnumerable<IHttpPartUploadInfo> partUploadInfos, string multiPartUploadResultHeaderName, IUploadIdentifier uploadIdentifier);
}
