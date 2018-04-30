using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sembium.ContentStorage.Storage.ContentNames;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContentNamesRepositorySettings>(configuration.GetSection("AppSettings").GetSection("ContentNamesRepository"));
        }
    }
}
