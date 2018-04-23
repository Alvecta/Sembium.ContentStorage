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
            //container.RegisterTypedFactory<IMultiPartUploadInfoFactory>().ForConcreteType<IMultiPartUploadInfo>(); use delegate
            builder.RegisterType<MultiPartUploadInfo>().As<IMultiPartUploadInfo>();

            //container.RegisterTypedFactory<IMultiPartIDUploadInfoFactory>().ForConcreteType<IMultiPartIDUploadInfo>(); use delegate
            builder.RegisterType<MultiPartIDUploadInfo>().As<IMultiPartIDUploadInfo>();

            //container.RegisterTypedFactory<IUploadIdentifierFactory>().ForConcreteType<IUploadIdentifier>(); use delegate
            builder.RegisterType<UploadIdentifier>().As<IUploadIdentifier>();

            //container.RegisterTypedFactory<IDownloadInfoFactory>().ForConcreteType<IDownloadInfo>(); use delegate
            builder.RegisterType<DownloadInfo>().As<IDownloadInfo>();

            //container.RegisterTypedFactory<IHttpPartUploadInfoFactory>().ForConcreteType<IHttpPartUploadInfo>(); use delegate
            builder.RegisterType<HttpPartUploadInfo>().As<IHttpPartUploadInfo>();

            builder.RegisterType<UploadIdentifierProvider>().As<IUploadIdentifierProvider>();
            builder.RegisterType<MonthHashAndCount>().As<IMonthHashAndCount>();


            //container.RegisterTypedFactory<IContentIdentifierFactory>().ForConcreteType<IContentIdentifier>();  replaced with ordinary factory for better performance

            /////container.RegisterTypedFactory<IDocumentIdentifierFactory>().ForConcreteType<IDocumentIdentifier>(); using delegate isntead
            builder.RegisterType<ContentNameProvider>().As<IContentNameProvider>();
            builder.RegisterType<ContentIdentifier>().As<IContentIdentifier>();
            builder.RegisterType<ContentIdentifierGenerator>().As<IContentIdentifierGenerator>();
            builder.RegisterType<ContentIdentifierSerializer>().As<IContentIdentifierSerializer>();
            builder.RegisterType<ContentIdentifiersProvider>().As<IContentIdentifiersProvider>();
            builder.RegisterType<ContentHashValidator>().As<IContentHashValidator>();
            builder.RegisterType<ContentsMonthHashRepository>().As<IContentsMonthHashRepository>();
            builder.RegisterType<SystemContainerProvider>().As<ISystemContainerProvider>();

            builder.RegisterType<HttpRequestInfo>().As<IHttpRequestInfo>();
        }
    }
}
