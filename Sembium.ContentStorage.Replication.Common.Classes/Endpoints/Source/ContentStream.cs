using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public class ContentStream : IContentStream
    {
        public long Size { get; }

        public Stream Stream { get; }

        public ContentStream(long size, System.IO.Stream stream)
        {
            Size = size;
            Stream = stream;
        }
    }
}
