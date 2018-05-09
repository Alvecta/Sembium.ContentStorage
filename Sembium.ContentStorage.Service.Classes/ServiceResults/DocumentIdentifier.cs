using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults
{
    public class DocumentIdentifier : IDocumentIdentifier
    {
        public string Hash { get; }
        public string Extension { get; }

        private DocumentIdentifier()
        {
            // do nothing
        }

        public DocumentIdentifier(string hash, string extension)
        {
            Hash = hash;
            Extension = extension;
        }
    }
}
