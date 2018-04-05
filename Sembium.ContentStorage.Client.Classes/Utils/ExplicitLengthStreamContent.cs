using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sembium.ContentStorage.Client.Utils
{
    public class ExplicitLengthStreamContent : System.Net.Http.StreamContent
    {
        private readonly long _explicitLength;

        public ExplicitLengthStreamContent(Stream stream, long explicitLength)
            : base(stream)
        {
            _explicitLength = explicitLength;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _explicitLength;
            return true;
        }
    }
}
