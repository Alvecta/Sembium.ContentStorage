using Sembium.ContentStorage.Replication.Common.Endpoints.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Destination
{
    public interface IDestination : IEndpoint
    {
        void PutContent(IContentIdentifier contentIdentifier, ISource source);
    }
}
