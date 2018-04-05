using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public delegate IDocumentIDUploadInfo IDocumentIDUploadInfoFactory(IIDUploadInfo idUploadInfo, string documentID);
}
