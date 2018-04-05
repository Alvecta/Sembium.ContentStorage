using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IDocumentIDUploadInfo
    {
        IIDUploadInfo IDUploadInfo { get; }
        string DocumentID { get; }
    }
}
