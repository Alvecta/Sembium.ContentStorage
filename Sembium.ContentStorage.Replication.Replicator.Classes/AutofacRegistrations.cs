using Autofac;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Replicator.CompleteMoment;
using Sembium.ContentStorage.Replication.Replicator.Config;
using Sembium.ContentStorage.Replication.Replicator.Main;

namespace Sembium.ContentStorage.Replication.Replicator
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<CommandLineConfigProvider>().As<IConfigProvider>();
            builder.RegisterType<EmptyConfigProvider>().As<IConfigProvider>();
            builder.RegisterType<CommandLineArgsConfigProvider>().As<ICommandLineArgsConfigProvider>();
            builder.RegisterType<FileConfigProvider>().As<IFileConfigProvider>();
            builder.RegisterType<UsageHelpProvider>().As<IUsageHelpProvider>();
            builder.RegisterType<FileStoredCompleteMomentProvider>().Named<ICompleteMomentProvider>("base");
            builder.RegisterType<Sembium.ContentStorage.Replication.Replicator.Main.Replicator>().Named<IReplicator>("base");
            builder.RegisterType<FileStoredCompleteMomentConfigProvider>().As<IFileStoredCompleteMomentConfigProvider>();
            builder.RegisterType<FileSystemCommandLineEndpointConfigProvider>().As<ICommandLineEndpointConfigProvider>();
            builder.RegisterType<ContentStorageCommandLineEndpointConfigProvider>().As<ICommandLineEndpointConfigProvider>();
            builder.RegisterType<Sembium.ContentStorage.Replication.Replicator.CompleteMoment.CompleteMomentInfo>().As<ICompleteMomentInfo>();
            builder.RegisterType<FileStoredCompleteMomentConfig>().As<IFileStoredCompleteMomentConfig>();
        }
    }
}
