using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Service.Library
{
    public static class WebHostBuilderInit
    {
        public static IWebHostBuilder InitWebHostBuilder(this IWebHostBuilder builder)
        {
            return
                builder
                    .UseStartup<Library.Startup>()
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        Library.ServicesConfig.ConfigureConfigurationBuilder(config);
                    });
        }
    }
}
