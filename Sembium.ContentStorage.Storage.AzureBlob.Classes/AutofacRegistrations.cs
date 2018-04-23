using Autofac;
using Microsoft.Extensions.Configuration;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder, IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.GetConnectionString("AzureBlobStorage")))
            {
                builder.RegisterType<AzureContainer>().As<Sembium.ContentStorage.Storage.Hosting.IContainer>();
                builder.RegisterType<AzureContent>().As<IContent>();
                builder.RegisterType<AzureContentNamesVault>().As<IContentNamesVault>();
                builder.RegisterType<AzureContentNamesVaultItem>().As<IAzureContentNamesVaultItem>();

                builder.RegisterType<AzureContentStorageHost>().As<IContentStorageHost>();
            }
        }
    }
}
