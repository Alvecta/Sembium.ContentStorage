using Autofac;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Service.ServiceResults;
using Sembium.ContentStorage.Service.ServiceResults.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<ContentStorageContainer>().As<IContentStorageContainer>();
            builder.RegisterType<ContentStorageUsersRepository>().As<IContentStorageUsersRepository>();
            builder.RegisterType<ContentStorage>().As<IContentStorage>();

            builder.RegisterType<DocumentUploadInfo>().As<IDocumentUploadInfo>();
            builder.RegisterType<DocumentMultiPartUploadInfo>().As<IDocumentMultiPartUploadInfo>();
            builder.RegisterType<DocumentIDUploadInfo>().As<IDocumentIDUploadInfo>();
            builder.RegisterType<DocumentMultiPartIDUploadInfo>().As<IDocumentMultiPartIDUploadInfo>();
            builder.RegisterType<UploadInfo>().As<IUploadInfo>();
            builder.RegisterType<IDUploadInfo>().As<IIDUploadInfo>();

            builder.RegisterType<ContentStorageAccount>().As<IContentStorageAccount>();
            builder.RegisterType<ContentStorageIdentity>().As<IContentStorageIdentity>();
            builder.RegisterType<ContentStoragePrincipal>().As<IContentStoragePrincipal>();
            builder.RegisterType<User>().As<IUser>();

            builder.RegisterType<ContainerState>().As<IContainerState>();
            builder.RegisterType<ContainerStateRepository>().As<IContainerStateRepository>();

            builder.RegisterType<ContentStorageContainer>().As<IContentStorageContainer>();

            builder.RegisterType<ContentStorageAccountProvider>().As<IContentStorageAccountProvider>();
            builder.RegisterType<ContentStorageSystemAccountProvider>().As<IContentStorageSystemAccountProvider>();
            builder.RegisterType<ContentStoragePrincipalProvider>().As<IContentStoragePrincipalProvider>();
            builder.RegisterType<ContentStorageSystemPrincipalProvider>().As<IContentStorageSystemPrincipalProvider>();

            builder.RegisterType<DocumentIDUploadInfoProvider>().As<IDocumentIDUploadInfoProvider>();
            builder.RegisterType<DocumentMultiPartIDUploadInfoProvider>().As<IDocumentMultiPartIDUploadInfoProvider>();
            builder.RegisterType<IDUploadInfoProvider>().As<IIDUploadInfoProvider>();
            builder.RegisterType<MultiPartIDUploadInfoProvider>().As<IMultiPartIDUploadInfoProvider>();

            builder.RegisterType<UploadIdentifierSerializer>().As<IUploadIdentifierSerializer>();

            builder.RegisterType<ContentStorageThreadPrincipalManager>().InstancePerLifetimeScope().As<IContentStoragePrincipalManager>();
            builder.RegisterType<AuthorizationChecker>().As<IAuthorizationChecker>();

            builder.RegisterType<SerializedObject>().As<ISerializedObject>();
            builder.RegisterType<DocumentIdentifier>().As<IDocumentIdentifier>();
            builder.RegisterType<DocumentIdentifierProvider>().As<IDocumentIdentifierProvider>();
            builder.RegisterType<DocumentIdentifierSerializer>().As<IDocumentIdentifierSerializer>();
            builder.RegisterType<DocumentIdentifierVersionSerializerProvider>().As<IDocumentIdentifierVersionSerializerProvider>();
            builder.RegisterType<DocumentIdentifierVersionSerializer<DocumentIdentifier>>()
                .Named<IDocumentIdentifierVersionSerializer>("DocumentIdentifierVersionSerializer1")   // not sure if 'named' is appropriate
                .As<IDocumentIdentifierVersionSerializer>();

            builder.RegisterType<ServiceModels.Users>().As<ServiceModels.IUsers>();
            builder.RegisterType<ServiceModels.System>().As<ServiceModels.ISystem>();
            builder.RegisterType<ServiceModels.Contents>().As<ServiceModels.IContents>();
        }
    }
}
