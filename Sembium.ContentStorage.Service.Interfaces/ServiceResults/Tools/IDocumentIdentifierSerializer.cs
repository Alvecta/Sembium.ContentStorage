using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public interface IDocumentIdentifierSerializer
    {
        string Serialize(IDocumentIdentifier documentIdentifier);
        IDocumentIdentifier Deserialize(string value);
    }
}
