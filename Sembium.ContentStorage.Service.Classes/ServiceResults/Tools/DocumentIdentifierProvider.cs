using Sembium.ContentStorage.Service.ServiceResults.Factories;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public class DocumentIdentifierProvider : IDocumentIdentifierProvider
    {
        private readonly IDocumentIdentifierFactory _documentIdentifierFactory;
        private readonly IContentIdentifierFactory _contentIdentifierFactory;
        private readonly IContentIdentifiersProvider _contentIdentifiersProvider;

        public DocumentIdentifierProvider(
            IDocumentIdentifierFactory documentIdentifierFactory,
            IContentIdentifierFactory contentIdentifierFactory,
            IContentIdentifiersProvider contentIdentifiersProvider)
        {
            _documentIdentifierFactory = documentIdentifierFactory;
            _contentIdentifierFactory = contentIdentifierFactory;
            _contentIdentifiersProvider = contentIdentifiersProvider;
        }

        public IDocumentIdentifier GetDocumentIdentifier(IContentIdentifier contentIdentifier)
        {
            return _documentIdentifierFactory(contentIdentifier.Hash, contentIdentifier.Extension);
        }

        public IContentIdentifier GetContentIdentifier(IContainer container, IDocumentIdentifier documentIdentifier)
        {
            return
                _contentIdentifiersProvider.GetContentIdentifiers(container, true, documentIdentifier.Hash)
                .Where(x => string.Equals(x.Extension, documentIdentifier.Extension, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }
    }
}
