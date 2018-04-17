using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.FileSystem.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Destination
{
    public class FileSystemDestination : FileSystemEndpoint, IFileSystemDestination
    {
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;

        public FileSystemDestination(
            string id,
            IContainer container,
            IHashProvider hashProvider,
            IHashStringProvider hashStringProvider,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IContentIdentifiersProvider contentIdentifiersProvider,
            IContentIdentifierGenerator contentIdentifierGenerator)
            : base(id, container, hashProvider, hashStringProvider, contentsMonthHashProvider, contentIdentifiersProvider)
        {
            _contentIdentifierGenerator = contentIdentifierGenerator;
        }

        public void PutContent(IContentIdentifier contentIdentifier, ISource source)
        {
            if (Container.ContentExists(contentIdentifier))
                return;

            var uncommittedContentIdentifier = _contentIdentifierGenerator.GetUncommittedContentIdentifier(contentIdentifier);

            var content = (Container.ContentExists(uncommittedContentIdentifier) ? Container.GetContent(uncommittedContentIdentifier) : Container.CreateContent(uncommittedContentIdentifier));

            var contentStream = source.GetContentStream(contentIdentifier);
            content.LoadFromStream(contentStream.Stream);

            Container.CommitContentAsync(uncommittedContentIdentifier).Wait();
        }
    }
}
