using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Base
{
    public class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            //////////container.RegisterTypedFactory<IFileSystemContainerFactory>().ForConcreteType<IContainer>("FileSystemContainer");
            //builder.RegisterType<FileSystemContainer>().Named<Sembium.ContentStorage.Storage.Hosting.IContainer>("FileSystemContainer").As<Sembium.ContentStorage.Storage.Hosting.IContainer>();

            ///////////container.RegisterTypedFactory<IFileSystemContentFactory>().ForConcreteType<IContent>("FileSystemContent");
            //builder.RegisterType<FileSystemContent>().Named<Sembium.ContentStorage.Storage.Hosting.IContent>("FileSystemContent").As<Sembium.ContentStorage.Storage.Hosting.IContent>();

            ////////container.RegisterTypedFactory<IFileSystemContentStorageHostFactory>().ForConcreteType<IContentStorageHost>("FileSystemContentStorageHost");
            //builder.RegisterType<FileSystemContentStorageHost>().Named<Sembium.ContentStorage.Storage.Hosting.IContentStorageHost>("FileSystemContentStorageHost").As<Sembium.ContentStorage.Storage.Hosting.IContentStorageHost>();

            //---------------
            builder.RegisterType<FileSystemContainer>().As<IFileSystemContainer>();
            builder.RegisterType<FileSystemContainerProvider>().As<IFileSystemContainerProvider>();
            builder.RegisterType<FileSystemContentStorageHost>().As<IFileSystemContentStorageHost>();
            builder.RegisterType<FileSystemFullFileNameProvider>().As<IFileSystemFullFileNameProvider>();
        }
    }
}
