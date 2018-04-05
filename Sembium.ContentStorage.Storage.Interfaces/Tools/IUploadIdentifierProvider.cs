using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public interface IUploadIdentifierProvider
    {
        IUploadIdentifier GetUploadIdentifier(IContentIdentifier contentIdentifier, string hostIdentifier);
        IContentIdentifier GetUncommittedContentIdentifier(IContainer container, IUploadIdentifier uploadIdentifier);
    }
}
