using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults.Factories
{
    public delegate IUploadIdentifier IUploadIdentifierFactory(string hash, string extension, string guid, string hostIdentifier);
}
