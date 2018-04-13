using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public class RouteReplicator : IRouteReplicator
    {
        private const int DefaultConnectionCountLimit = 20;

        private readonly ICompleteMomentProvider _completeMomentProvider;

        public RouteReplicator(ICompleteMomentProvider completeMomentProvider)
        {
            _completeMomentProvider = completeMomentProvider;
        }

        public void ReplicateRouteContents(IRoute route, IEnumerable<IContentIdentifier> contentIdentifiers, int connectionCountLimit)
        {
            if (_completeMomentProvider == null)
            {
                Task.WhenAll(contentIdentifiers.Select(x => ReplicateContentAsync(route, x))).Wait();
            }
            else
            {
                var aTaskFaulted = false;

                var headedTaskQueue = new HeadedTaskQueue<DateTimeOffset>(
                    (connectionCountLimit <= 0 ? DefaultConnectionCountLimit : connectionCountLimit),
                    (task) =>
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            aTaskFaulted = true;
                        }

                        if (!aTaskFaulted)
                        {
                            _completeMomentProvider.SetCompleteMoment(route.Source.ID, route.Destination.ID, task.Result);
                        }
                    });

                foreach (var contentIdentifier in contentIdentifiers)
                {
                    headedTaskQueue.Add(ReplicateContentAsync(route, contentIdentifier));
                }

                headedTaskQueue.Wait();
            }
        }

        private Task<DateTimeOffset> ReplicateContentAsync(IRoute route, IContentIdentifier contentIdentifier)
        {
            return Task.Factory.StartNew(() => ReplicateContent(route, contentIdentifier));
        }

        private DateTimeOffset ReplicateContent(IRoute route, IContentIdentifier contentIdentifier)
        {
            route.Destination.PutContent(contentIdentifier, route.Source);
            return contentIdentifier.ModifiedMoment;
        }
    }
}
