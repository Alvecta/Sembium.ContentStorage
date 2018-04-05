using Sembium.ContentStorage.Service.ServiceResults;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class DocumentMultiPartUploadInfo : IDocumentMultiPartUploadInfo
    {
        public IMultiPartUploadInfo MultiPartUploadInfo { get; private set; }
        public IDocumentIdentifier DocumentIdentifier { get; private set; }

        public DocumentMultiPartUploadInfo(IMultiPartUploadInfo multiPartUploadInfo, IDocumentIdentifier documentIdentifier)
        {
            MultiPartUploadInfo = multiPartUploadInfo;
            DocumentIdentifier = documentIdentifier;
        }
    }
}
