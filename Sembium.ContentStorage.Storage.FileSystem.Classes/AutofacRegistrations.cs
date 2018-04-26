using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<FileSystemContainer>().As<IFileSystemContainer>();
            builder.RegisterType<FileSystemContainerProvider>().As<IFileSystemContainerProvider>();
            builder.RegisterType<FileSystemContentStorageHost>().As<IFileSystemContentStorageHost>();
            builder.RegisterType<FileSystemFullFileNameProvider>().As<IFileSystemFullFileNameProvider>();


            if (!string.IsNullOrEmpty(configuration.GetSection("AppSettings").GetValue<string>("StorageRoot")))
            {
                builder.RegisterType<FileSystemURLContent>().As<IContent>();
                builder.RegisterType<TransferServiceProvider>().As<ITransferServiceProvider>();

                builder.RegisterType<FileSystemStorageRootProvider>().As<IFileSystemStorageRootProvider>();
                builder.RegisterType<FileSystemContentStorageHost>()
                    .As<IContentStorageHost>()
                    .WithParameter(new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "storageRoot",
                        (pi, ctx) => ctx.Resolve<IFileSystemStorageRootProvider>().GetRoot()));
            }
        }
    }
}
