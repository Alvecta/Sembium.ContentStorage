using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Factories
{
    public delegate IDocumentIdentifier IDocumentIdentifierFactory(string hash, string extension);
}
