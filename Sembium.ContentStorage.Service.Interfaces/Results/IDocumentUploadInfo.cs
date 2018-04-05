using Sembium.ContentStorage.Service.ServiceResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IDocumentUploadInfo
    {
        IUploadInfo UploadInfo { get; }
        IDocumentIdentifier DocumentIdentifier { get; }
    }
}
