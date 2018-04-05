using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<Amazon.S3.IAmazonS3>();
        }

        public static void Configure(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddAWSProvider(configuration.GetAWSLoggingConfigSection());
        }

        public static void ConfigureConfigurationBuilder(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddAmazonElasticBeanstalk();
        }
    }
}
