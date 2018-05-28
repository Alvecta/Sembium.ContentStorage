using Autofac;
using Sembium.ContentStorage.Replication.Common.Endpoints.Destination;
using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Replication.FileSystem.Config;
using Sembium.ContentStorage.Replication.FileSystem.Endpoints.Destination;
using Sembium.ContentStorage.Replication.FileSystem.Endpoints.Source;
using Sembium.ContentStorage.Storage.FileSystem;
using Sembium.ContentStorage.Storage.Hosting;

namespace Sembium.ContentStorage.Replication.FileSystem
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<FileSystemContent>().As<IContent>();

            builder.RegisterType<FileSystemEndpointConfig>().As<IFileSystemEndpointConfig>();

            builder.RegisterType<FileSystemSource>().As<ISource>();

            builder.RegisterType<FileSystemDestination>().As<IFileSystemDestination>();

            builder.RegisterType<FileSystemSourceResolver>().Named<ISourceResolver>("base");
            builder.RegisterType<FileSystemDestinationResolver>().Named<IDestinationResolver>("base");
        }
    }
}
