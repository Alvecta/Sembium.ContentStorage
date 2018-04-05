using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IUploadIdentifierSerializer
    {
        string Serialize(IUploadIdentifier uploadIdentifier);
        IUploadIdentifier Deserialize(string value);
    }
}
