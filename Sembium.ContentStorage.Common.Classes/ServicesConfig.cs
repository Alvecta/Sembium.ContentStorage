using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Common
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContentNamesRepositorySettings>(configuration.GetSection("AppSettings").GetSection("ContentNamesRepository"));
        }
    }
}
