using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class UploadIdentifier : IUploadIdentifier
    {
        public string Hash { get; private set; }
        public string Extension { get; private set; }
        public string Guid { get; private set; }
        public string HostIdentifier { get; private set; }

        public UploadIdentifier(string hash, string extension, string guid, string hostIdentifier)
        {
            Hash = hash;
            Extension = extension;
            Guid = guid;
            HostIdentifier = hostIdentifier;
        }
    }
}
