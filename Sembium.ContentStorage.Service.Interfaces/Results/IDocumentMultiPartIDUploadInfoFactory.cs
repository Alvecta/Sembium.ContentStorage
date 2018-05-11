using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public delegate IDocumentMultiPartIDUploadInfo IDocumentMultiPartIDUploadInfoFactory(IMultiPartIDUploadInfo multiPartIDUploadInfo, string documentID);
}
