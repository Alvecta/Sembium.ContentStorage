using Autofac;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.Common.Main;
using Sembium.ContentStorage.Replication.Common.Route;

namespace Sembium.ContentStorage.Replication.Common
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<Sembium.ContentStorage.Replication.Common.Route.Route>().As<IRoute>();
            builder.RegisterType<Sembium.ContentStorage.Replication.Common.Config.RouteConfig>().As<IRouteConfig>();
            builder.RegisterType<Sembium.ContentStorage.Replication.Common.Config.Config>().As<IConfig>();
            builder.RegisterType<ConfigResolver>().As<IConfigResolver>();
            builder.RegisterType<Sembium.ContentStorage.Replication.Common.Endpoints.Source.ContentStream>().As<IContentStream>();
            builder.RegisterType<SourceResolverProvider>().As<ISourceResolverProvider>();
            builder.RegisterType<DestinationResolverProvider>().As<IDestinationResolverProvider>();
            builder.RegisterType<RouteProvider>().Named<IRouteProvider>("base");
            builder.RegisterType<MergeConfigProvider>().As<IMergeConfigProvider>();
            builder.RegisterType<MergeRouteConfigProvider>().As<IMergeRouteConfigProvider>();
            builder.RegisterType<ReplicationWorker>().As<IReplicationWorker>();
            builder.RegisterType<RouteWorker>().Named<IRouteWorker>("base");
            builder.RegisterType<RouteReplicator>().Named<IRouteReplicator>("base");
        }
    }
}
