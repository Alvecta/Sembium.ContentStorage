using Autofac;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.ContentStorage.Config;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Destination;
using Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source;

namespace Sembium.ContentStorage.Replication.ContentStorage
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<ContentStorageEndpointConfig>().As<IContentStorageEndpointConfig>();

            builder.RegisterType<ContentStorageSource>().As<IContentStorageSource>();

            builder.RegisterType<ContentStorageDestination>().As<IContentStorageDestination>();

            builder.RegisterType<ContentStorageSourceResolver>().Named<ISourceResolver>("base");
            builder.RegisterType<ContentStorageDestinationResolver>().Named<IDestinationResolver>("base");

            builder.RegisterType<ContentStorageServiceProvider>().As<IContentStorageServiceProvider>();
        }
    }
}
