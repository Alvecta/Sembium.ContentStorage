using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public interface IDocumentIdentifierVersionSerializerProvider
    {
        IDocumentIdentifierVersionSerializer GetSerializer(IDocumentIdentifier documentIdentifier);
        IDocumentIdentifierVersionSerializer GetSerializer(string documentIdentifierTypeName);
    }
}
