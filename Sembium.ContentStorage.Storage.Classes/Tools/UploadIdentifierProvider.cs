using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public class UploadIdentifierProvider : IUploadIdentifierProvider
    {
        private readonly IUploadIdentifierFactory _uploadIdentifierFactory;
        private readonly IContentIdentifierFactory _contentIdentifierFactory;
        private readonly IContentIdentifiersProvider _contentIdentifiersProvider;

        public UploadIdentifierProvider(
            IUploadIdentifierFactory uploadIdentifierFactory,
            IContentIdentifierFactory contentIdentifierFactory,
            IContentIdentifiersProvider contentIdentifiersProvider)
        {
            _uploadIdentifierFactory = uploadIdentifierFactory;
            _contentIdentifierFactory = contentIdentifierFactory;
            _contentIdentifiersProvider = contentIdentifiersProvider;
        }

        public IUploadIdentifier GetUploadIdentifier(IContentIdentifier contentIdentifier, string hostIdentifier)
        {
            return _uploadIdentifierFactory(contentIdentifier.Hash, contentIdentifier.Extension, contentIdentifier.Guid, hostIdentifier);
        }

        public IContentIdentifier GetUncommittedContentIdentifier(IContainer container, IUploadIdentifier uploadIdentifier)
        {
            return
                _contentIdentifiersProvider.GetContentIdentifiers(container, false, uploadIdentifier.Hash)
                .Where(x => string.Equals(x.Extension, uploadIdentifier.Extension, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => string.Equals(x.Guid, uploadIdentifier.Guid, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }
    }
}
