using Autofac;
using Microsoft.Extensions.Configuration;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder, IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.GetSection("AppSettings").GetValue<string>("StorageRoot")))
            {
                builder.RegisterType<FileSystemURLContent>().As<IContent>();
                builder.RegisterType<FileSystemStorageRootProvider>().As<IFileSystemStorageRootProvider>();
                builder.RegisterType<TransferServiceProvider>().As<ITransferServiceProvider>();

                builder.RegisterType<FileSystemContentStorageHost>().As<IContentStorageHost>();
            }
        }
    }
}
