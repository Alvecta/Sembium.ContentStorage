using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sembium.ContentStorage.Utils.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Library
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Sembium.ContentStorage.Common.ServicesConfig.ConfigureServices(services, configuration);
            Sembium.ContentStorage.Storage.AmazonS3.ServicesConfig.ConfigureServices(services, configuration);
        }

        public static void Configure(ILoggerFactory loggerFactory, IConfiguration configuration)
        {

        }

        public static void ConfigureConfigurationBuilder(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddEnvironmentVariablesAsSettings();
        }
    }
}
