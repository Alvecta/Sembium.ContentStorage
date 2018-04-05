using Autofac;
using Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization;
using Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Utils;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            Sembium.ContentStorage.Replication.Common.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Replication.FileSystem.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Replication.ContentStorage.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Replication.Replicator.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Replication.Logging.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Client.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Common.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Storage.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Storage.FileSystem.Base.AutofacRegistrations.RegisterFor(builder);

            builder.RegisterType<LoggingConfigurator>().As<ILoggingConfigurator>();
            builder.RegisterType<MainService>().As<IMainService>();
            builder.RegisterType<LoggerLogger>().As<Sembium.ContentStorage.Common.ILogger>();
        }
    }
}
