using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public class DocumentIdentifierSerializer : IDocumentIdentifierSerializer
    {
        private readonly ISerializer _serializer;
        private readonly IDocumentIdentifierVersionSerializerProvider _documentIdentifierVersionSerializerProvider;
        private readonly ISerializedObjectFactory _serializedObjectFactory;

        public DocumentIdentifierSerializer(
            ISerializer serializer,
            IDocumentIdentifierVersionSerializerProvider documentIdentifierVersionSerializerProvider,
            ISerializedObjectFactory serializedObjectFactory)
        {
            _serializer = serializer;
            _documentIdentifierVersionSerializerProvider = documentIdentifierVersionSerializerProvider;
            _serializedObjectFactory = serializedObjectFactory;
        }

        public string Serialize(IDocumentIdentifier documentIdentifier)
        {
            if (documentIdentifier == null)
                return null;

            var documentIdentifierVersionSerializer = _documentIdentifierVersionSerializerProvider.GetSerializer(documentIdentifier);

            var serializedDocumentIdentifier = documentIdentifierVersionSerializer.Serialize(documentIdentifier);

            var serializedObject = _serializedObjectFactory(documentIdentifierVersionSerializer.DocumentIdentifierTypeName, serializedDocumentIdentifier); 

            return _serializer.Serialize(serializedObject);
        }

        public IDocumentIdentifier Deserialize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var serializedObject = _serializer.Deserialize<ISerializedObject>(value);

            var documentIdentifierVersionSerializer = _documentIdentifierVersionSerializerProvider.GetSerializer(serializedObject.DataTypeName);

            return documentIdentifierVersionSerializer.Deserialize(serializedObject.Data);
        }
    }
}
