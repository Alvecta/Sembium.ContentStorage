using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public class ContentIdentifierSerializer : IContentIdentifierSerializer
    {
        private readonly ISerializer _serializer;

        public ContentIdentifierSerializer(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string Serialize(IContentIdentifier contentIdentifier)
        {
            return _serializer.Serialize(contentIdentifier);
        }

        public IContentIdentifier Deserialize(string value)
        {
            return _serializer.Deserialize<ContentIdentifier>(value);
        }
    }
}
