using Sembium.ContentStorage.Common.Utils;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Route
{
    public class RouteWorker : IRouteWorker
    {
        private readonly IRouteProvider _routeProvider;
        private readonly ICompleteMomentProvider _completeMomentProvider;
        private readonly IRouteReplicator _routeReplicator;

        public RouteWorker(
            IRouteProvider routeProvider,
            ICompleteMomentProvider completeMomentProvider,
            IRouteReplicator routeReplicator)
        {
            _routeProvider = routeProvider;
            _completeMomentProvider = completeMomentProvider;
            _routeReplicator = routeReplicator;
        }

        public void RunRoute(IRouteConfig routeConfig)
        {
            try
            {
                var route = _routeProvider.GetRoute(routeConfig);

                var completeMoment = (routeConfig.ForceAllContents ? DateTimeOffset.MinValue : _completeMomentProvider.GetCompleteMoment(route.Source.ID, route.Destination.ID));

                DateTimeOffset sourceLastModifiedMoment;
                var contentIdentifiersList = GetReplicateContentIdentifiers(routeConfig, route, completeMoment, out sourceLastModifiedMoment).ToList();

                if (contentIdentifiersList.Any())
                {
                    _routeReplicator.ReplicateRouteContents(route, contentIdentifiersList, routeConfig.ConnectionCountLimit);
                }
                else
                {
                    var newCompleteMoment = new[] { completeMoment, sourceLastModifiedMoment }.Max();
                    _completeMomentProvider.SetCompleteMoment(route.Source.ID, route.Destination.ID, newCompleteMoment);
                }
            }
            finally
            {
                _completeMomentProvider.Finish();
            }
        }

        private IEnumerable<IContentIdentifier> GetReplicateContentIdentifiers(IRouteConfig routeConfig, IRoute route, DateTimeOffset completeMoment, out DateTimeOffset sourceLastModifiedMoment)
        {
            var sourceContentIdentifiersTask = route.Source.GetContentIdentifiersAsync(completeMoment);

            if (!routeConfig.ParallelGetLists)
            {
                sourceContentIdentifiersTask.Wait();
            }

            var destinationContentIdentifiersTask = route.Destination.GetContentIdentifiersAsync(completeMoment);

            Task.WaitAll(sourceContentIdentifiersTask, destinationContentIdentifiersTask);

            var sourceContentIdentifiers = sourceContentIdentifiersTask.Result.ToList();  // ToList() to get the list once
            var destinationContentIdentifiers = destinationContentIdentifiersTask.Result.ToList();  // ToList() to get the list once

            sourceLastModifiedMoment = sourceContentIdentifiers.Select(x => x.ModifiedMoment).LastOrDefault();

            if (!routeConfig.SkipDestinationCheck)
            {
                var extraContentIdentifiers = destinationContentIdentifiers.Except(sourceContentIdentifiers);

                if (extraContentIdentifiers.Any())
                {
                    var contentIdentifier = extraContentIdentifiers.First();
                    throw new UserException(string.Format("Unknown destination content: {0}/{1}", contentIdentifier.Hash, contentIdentifier.Extension));
                }
            }

            var contentIdentifiers = sourceContentIdentifiers.Except(destinationContentIdentifiers);

            if (routeConfig.ContentCountLimit > 0)
            {
                contentIdentifiers = contentIdentifiers.Take(routeConfig.ContentCountLimit);
            }

            return contentIdentifiers;
        }

        public void HashCheckRoute(IRouteConfig routeConfig)
        {
            var route = _routeProvider.GetRoute(routeConfig);

            var sourceContentsHashTask = route.Source.GetContentsHashAsync(routeConfig.HashCheckMoment.Value);
            var destinationContentsHashTask = route.Destination.GetContentsHashAsync(routeConfig.HashCheckMoment.Value);

            Task.WaitAll(sourceContentsHashTask, destinationContentsHashTask);

            var sourceContentsHash = sourceContentsHashTask.Result;
            var destinationContentsHash = destinationContentsHashTask.Result;

            if (!string.Equals(sourceContentsHash, destinationContentsHash, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UserException("Hashes do not match.");
            }
        }
    }
}
