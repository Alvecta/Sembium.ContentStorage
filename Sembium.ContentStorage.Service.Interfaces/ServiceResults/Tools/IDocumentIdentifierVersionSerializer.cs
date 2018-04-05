using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public interface IDocumentIdentifierVersionSerializer 
    {
        string DocumentIdentifierTypeName { get; }
        string Serialize(IDocumentIdentifier documentIdentifier);
        IDocumentIdentifier Deserialize(string input);
    }
}
