using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IDocumentMultiPartIDUploadInfo
    {
        IMultiPartIDUploadInfo MultiPartIDUploadInfo { get; }
        string DocumentID { get; }
    }
}
