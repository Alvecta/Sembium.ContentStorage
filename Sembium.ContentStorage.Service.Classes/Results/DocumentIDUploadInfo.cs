using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class DocumentIDUploadInfo : IDocumentIDUploadInfo
    {
        public IIDUploadInfo IDUploadInfo { get; private set; }
        public string DocumentID { get; private set; }

        public DocumentIDUploadInfo(IIDUploadInfo idUploadInfo, string documentID)
        {
            IDUploadInfo = idUploadInfo;
            DocumentID = documentID;
        }
    }
}
