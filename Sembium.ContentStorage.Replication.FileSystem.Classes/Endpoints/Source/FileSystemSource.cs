using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.FileSystem.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Source
{
    public class FileSystemSource : FileSystemEndpoint, ISource
    {
        private readonly IContentStreamFactory _contentStreamFactory;

        public FileSystemSource(string id, 
            IContainer container,
            IHashProvider hashProvider,
            IHashStringProvider hashStringProvider,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IContentStreamFactory contentStreamFactory)
            : base(id, container, hashProvider, hashStringProvider, contentsMonthHashProvider)
        {
            _contentStreamFactory = contentStreamFactory;
        }

        public IContentStream GetContentStream(IContentIdentifier contentIdentifier)
        {
            var content = Container.GetContent(contentIdentifier);
            return _contentStreamFactory(content.GetSize(), content.GetReadStream());
        }
    }
}
