using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public interface IRouteReplicator
    {
        void ReplicateRouteContents(IRoute route, IEnumerable<IContentIdentifier> contentIdentifiers, int connectionCountLimit);
    }
}
