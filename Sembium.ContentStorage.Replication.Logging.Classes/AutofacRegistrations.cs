using Autofac;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.Common.Route;
using Sembium.ContentStorage.Replication.Logging.CompleteMoment;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Logging.Endpoints.Source;
using Sembium.ContentStorage.Replication.Logging.Main;
using Sembium.ContentStorage.Replication.Logging.Route;
using Sembium.ContentStorage.Replication.Replicator.Main;

namespace Sembium.ContentStorage.Replication.Logging
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<LoggingSource>().As<ILoggingSource>();
            builder.RegisterType<LoggingContentStorageSource>().As<ILoggingContentStorageSource>();
            builder.RegisterType<LoggingDestination>().As<ILoggingDestination>();
            builder.RegisterType<LoggingSourceResolver>().Named<ISourceResolver>("LoggingSourceResolver");
            builder.RegisterType<LoggingDestinationResolver>().Named<IDestinationResolver>("LoggingDestinationResolver");

            builder.RegisterType<LoggingReplicator>().Named<IReplicator>("LoggingReplicator");
            builder.RegisterType<LoggingRouteWorker>().Named<IRouteWorker>("LoggingRouteWorker");
            builder.RegisterType<LoggingRouteReplicator>().Named<IRouteReplicator>("LoggingRouteReplicator");
            builder.RegisterType<LoggingRouteProvider>().Named<IRouteProvider>("LoggingRouteProvider");
            builder.RegisterType<LoggingCompleteMomentProvider>().Named<ICompleteMomentProvider>("LoggingCompleteMomentProvider");

            builder.RegisterType<LogFileNameProvider>().As<ILogFileNameProvider>();

            builder.RegisterDecorator<ISourceResolver>((x, inner) => x.ResolveNamed<ISourceResolver>("LoggingSourceResolver", TypedParameter.From(inner)), "base").As<ISourceResolver>();
            builder.RegisterDecorator<IDestinationResolver>((x, inner) => x.ResolveNamed<IDestinationResolver>("LoggingDestinationResolver", TypedParameter.From(inner)), "base").As<IDestinationResolver>();

            builder.RegisterDecorator<IReplicator>((x, inner) => x.ResolveNamed<IReplicator>("LoggingReplicator", TypedParameter.From(inner)), "base").As<IReplicator>();
            builder.RegisterDecorator<IRouteWorker>((x, inner) => x.ResolveNamed<IRouteWorker>("LoggingRouteWorker", TypedParameter.From(inner)), "base").As<IRouteWorker>();
            builder.RegisterDecorator<IRouteReplicator>((x, inner) => x.ResolveNamed<IRouteReplicator>("LoggingRouteReplicator", TypedParameter.From(inner)), "base").As<IRouteReplicator>();
            builder.RegisterDecorator<IRouteProvider>((x, inner) => x.ResolveNamed<IRouteProvider>("LoggingRouteProvider", TypedParameter.From(inner)), "base").As<IRouteProvider>();
            builder.RegisterDecorator<ICompleteMomentProvider>((x, inner) => x.ResolveNamed<ICompleteMomentProvider>("LoggingCompleteMomentProvider", TypedParameter.From(inner)), "base").As<ICompleteMomentProvider>();
        }
    }
}
