using Sembium.ContentStorage.Service.ServiceResults;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public delegate IDocumentMultiPartUploadInfo IDocumentMultiPartUploadInfoFactory(IMultiPartUploadInfo multiPartUploadInfo, IDocumentIdentifier documentIdentifier);
}
