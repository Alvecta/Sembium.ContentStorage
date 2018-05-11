using Autofac;
using Sembium.ContentStorage.Common.ContentNames;
using Sembium.ContentStorage.Common.ContentNames.Vault;
using Sembium.ContentStorage.Common.MonthHash;
using Sembium.ContentStorage.Utils.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<MultiPartIDUploadInfo>().As<IMultiPartIDUploadInfo>();
            builder.RegisterType<ContentIdentifierSerializer>().As<IContentIdentifierSerializer>();

            builder.RegisterType<ContentsMonthHashRepository>().As<IContentsMonthHashRepository>();
            builder.RegisterType<ContentMonthProvider>().As<IContentMonthProvider>();
            builder.RegisterType<ContentsMonthHashProvider>().As<IContentsMonthHashProvider>();
            builder.RegisterType<MonthHashAndCount>().As<IMonthHashAndCount>();

            builder.RegisterType<ContentNamesRepository>().As<IContentNamesRepository>();
            builder.RegisterType<ContentNamesVault>().As<IContentNamesVault>();
            builder.RegisterType<ContentNamesVaultItem>().As<IContentNamesVaultItem>();

            builder.RegisterOptionsAdapter<ContentNamesRepositorySettings>().As<IContentNamesRepositorySettings>();
        }
    }
}
