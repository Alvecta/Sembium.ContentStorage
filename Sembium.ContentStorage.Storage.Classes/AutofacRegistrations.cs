using Autofac;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<MultiPartUploadInfo>().As<IMultiPartUploadInfo>();
            builder.RegisterType<UploadIdentifier>().As<IUploadIdentifier>();
            builder.RegisterType<DownloadInfo>().As<IDownloadInfo>();
            builder.RegisterType<HttpPartUploadInfo>().As<IHttpPartUploadInfo>();

            builder.RegisterType<UploadIdentifierProvider>().As<IUploadIdentifierProvider>();

            builder.RegisterType<ContentNameProvider>().As<IContentNameProvider>();
            builder.RegisterType<ContentIdentifier>().As<IContentIdentifier>();
            builder.RegisterType<ContentIdentifierGenerator>().As<IContentIdentifierGenerator>();
            builder.RegisterType<ContentIdentifiersProvider>().As<IContentIdentifiersProvider>();
            builder.RegisterType<ContentHashValidator>().As<IContentHashValidator>();

            builder.RegisterType<SystemContainerProvider>().Named<ISystemContainerProvider>("base");
            builder.RegisterType<CachingSystemContainerProvider>().Named<ISystemContainerProvider>("caching");

            builder.RegisterDecorator<ISystemContainerProvider>((x, inner) => x.ResolveNamed<ISystemContainerProvider>("caching", TypedParameter.From(inner)), "base").As<ISystemContainerProvider>();

            builder.RegisterType<HttpRequestInfo>().As<IHttpRequestInfo>();
        }
    }
}
