using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public class DocumentIdentifierVersionSerializer<T> : IDocumentIdentifierVersionSerializer where T : DocumentIdentifier
    {
        public string DocumentIdentifierTypeName
        {
            get { return typeof(T).Name; }
        }

        private readonly ISerializer _serializer;

        public DocumentIdentifierVersionSerializer(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string Serialize(IDocumentIdentifier documentIdentifier)
        {
            T x = documentIdentifier as T;

            if (x == null)
            {
                throw new UserException("Invalid serialized document identifer type");
            }

            return _serializer.Serialize(x);
        }

        public IDocumentIdentifier Deserialize(string value)
        {
            return _serializer.Deserialize<T>(value);
        }
    }
}
