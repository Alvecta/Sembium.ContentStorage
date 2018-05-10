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
        public IMultiPartIDUploadInfo MultiPartIDUploadInfo { get; }
        public string DocumentID { get; }

        public DocumentMultiPartIDUploadInfo(IMultiPartIDUploadInfo multiPartIDUploadInfo, string documentID)
        {
            MultiPartIDUploadInfo = multiPartIDUploadInfo;
            DocumentID = documentID;
        }
    }
}
