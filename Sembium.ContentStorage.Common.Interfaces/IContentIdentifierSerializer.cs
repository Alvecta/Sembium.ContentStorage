using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentIdentifierSerializer
    {
        string Serialize(IContentIdentifier contentIdentifier);
        IContentIdentifier Deserialize(string value);
    }
}
