using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public interface IDocumentIdentifierProvider
    {
        IDocumentIdentifier GetDocumentIdentifier(IContentIdentifier contentIdentifier);
        IContentIdentifier GetContentIdentifier(IContainer container, IDocumentIdentifier documentIdentifier);
    }
}
