using Autofac;
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
            builder.RegisterType<ContentNamesRepository>().As<IContentNamesRepository>();
            builder.RegisterType<MonthHashAndCount>().As<IMonthHashAndCount>();
        }
    }
}
