using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Factories
{
    public delegate IMultiPartIDUploadInfo IMultiPartIDUploadInfoFactory(string httpMethod, long partSize, IEnumerable<IHttpPartUploadInfo> partUploadInfos, string multiPartUploadResultHeaderName, string uploadID);
}
