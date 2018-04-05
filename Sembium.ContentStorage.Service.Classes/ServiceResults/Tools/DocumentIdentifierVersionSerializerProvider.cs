using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public class DocumentIdentifierVersionSerializerProvider : IDocumentIdentifierVersionSerializerProvider
    {
        private readonly IEnumerable<IDocumentIdentifierVersionSerializer> _documentIdentifierVersionSerializers;

        public DocumentIdentifierVersionSerializerProvider(
            IEnumerable<IDocumentIdentifierVersionSerializer> documentIdentifierVersionSerializers)
        {
            _documentIdentifierVersionSerializers = documentIdentifierVersionSerializers;
        }

        public IDocumentIdentifierVersionSerializer GetSerializer(IDocumentIdentifier documentIdentifier)
        {
            return GetSerializer(documentIdentifier.GetType().Name);
        }

        public IDocumentIdentifierVersionSerializer GetSerializer(string documentIdentifierTypeName)
        {
            return _documentIdentifierVersionSerializers.Where(x => string.Equals(x.DocumentIdentifierTypeName, documentIdentifierTypeName)).Single();
        }
    }
}
