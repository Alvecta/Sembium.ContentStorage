using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Library
{
    public static class AutofacRegistrations
    {
        public static IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterFor(builder, configuration);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        public static void RegisterFor(ContainerBuilder builder, IConfiguration configuration)
        {
            Sembium.ContentStorage.Common.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Misc.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Service.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Storage.AutofacRegistrations.RegisterFor(builder);

            Sembium.ContentStorage.Storage.AmazonS3.AutofacRegistrations.RegisterFor(builder, configuration);

            Sembium.ContentStorage.Storage.AzureBlob.AutofacRegistrations.RegisterFor(builder, configuration);

            Sembium.ContentStorage.Storage.FileSystem.AutofacRegistrations.RegisterFor(builder, configuration);
            Sembium.ContentStorage.Storage.FileSystem.Transfer.AutofacRegistrations.RegisterFor(builder);
        }
    }
}