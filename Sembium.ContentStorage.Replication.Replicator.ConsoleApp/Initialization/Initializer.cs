using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization
{
    public static class Initializer
    {
        public static IServiceProvider GetServiceProvider()
        {
            var startup = new Startup();
            var serviceProvider = startup.ConfigureServices(new ServiceCollection());

            var loggingFactory = serviceProvider.GetService<ILoggerFactory>();
            var loggingConfigurator = serviceProvider.GetService<ILoggingConfigurator>();
            loggingConfigurator.Configure(loggingFactory);

            return serviceProvider;
        }
    }
}
