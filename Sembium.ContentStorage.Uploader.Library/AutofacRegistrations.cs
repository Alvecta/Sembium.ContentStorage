using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Uploader.AmazonLambda
{
    public static class AutofacRegistrations
    {
        public static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterFor(builder);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        public static void RegisterFor(ContainerBuilder builder)
        {
            Sembium.ContentStorage.Client.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Common.AutofacRegistrations.RegisterFor(builder);
            Sembium.ContentStorage.Storage.AutofacRegistrations.RegisterFor(builder);
        }
    }
}