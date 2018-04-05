using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization
{
    public class Startup
    {
        public Startup()
        {
            var basePath = Path.GetDirectoryName(Environment.GetCommandLineArgs().First());

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                //////////////////.AddXmlFile("app.config", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddOptions();
            services.AddLogging(ConfigureLogging);

            var builder = new ContainerBuilder();

            builder.Populate(services);
            AutofacRegistrations.RegisterFor(builder);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        private void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddConfiguration(Configuration.GetSection("Logging"));
            builder.SetMinimumLevel(LogLevel.Trace);
        }
    }
}
