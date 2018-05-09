using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class UploadIdentifier : IUploadIdentifier
    {
        public string Hash { get; }
        public string Extension { get; }
        public string Guid { get; }
        public string HostIdentifier { get; }

        private UploadIdentifier()
        {
            // do nothing
        }

        public UploadIdentifier(string hash, string extension, string guid, string hostIdentifier)
        {
            Hash = hash;
            Extension = extension;
            Guid = guid;
            HostIdentifier = hostIdentifier;
        }
    }
}
