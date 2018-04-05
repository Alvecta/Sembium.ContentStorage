using Sembium.ContentStorage.Replication.Common.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public interface ISource : IEndpoint
    {
        IContentStream GetContentStream(IContentIdentifier contentIdentifier);
    }
}
