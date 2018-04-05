using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class DocumentMultiPartIDUploadInfo : IDocumentMultiPartIDUploadInfo
    {
        public IMultiPartIDUploadInfo MultiPartIDUploadInfo { get; private set; }
        public string DocumentID { get; private set; }

        public DocumentMultiPartIDUploadInfo(IMultiPartIDUploadInfo multiPartIDUploadInfo, string documentID)
        {
            MultiPartIDUploadInfo = multiPartIDUploadInfo;
            DocumentID = documentID;
        }
    }
}
