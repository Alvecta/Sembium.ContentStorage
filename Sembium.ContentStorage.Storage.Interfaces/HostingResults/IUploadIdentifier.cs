using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public interface IUploadIdentifier
    {
        string Hash { get; }
        string Extension { get; }
        string Guid { get; }
        string HostIdentifier { get; }
    }
}
