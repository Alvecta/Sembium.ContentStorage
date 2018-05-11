using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IMultiPartIDUploadInfoProvider
    {
        IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(IMultiPartUploadInfo multiPartUploadInfo);
    }
}
