using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder, IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.GetSection("AppSettings").GetValue<string>("AmazonS3BucketName")))
            {
                builder.RegisterType<AmazonContainer>().As<Sembium.ContentStorage.Storage.Hosting.IContainer>();
                builder.RegisterType<AmazonContent>().As<Sembium.ContentStorage.Storage.Hosting.IContent>();

                builder.RegisterType<AmazonContentStorageHost>().As<Sembium.ContentStorage.Storage.Hosting.IContentStorageHost>();
            }
        }
    }
}
