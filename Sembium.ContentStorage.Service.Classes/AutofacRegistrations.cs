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
            /////container.RegisterTypedFactory<IContentStorageContainerFactory>().ForConcreteType<IContentStorageContainer>(); use delegate instead
            builder.RegisterType<ContentStorageContainer>().As<IContentStorageContainer>();

            /////container.RegisterTypedFactory<IContentStorageUsersRepositoryFactory>().ForConcreteType<IContentStorageUsersRepository>(); use delegate instead
            builder.RegisterType<ContentStorageUsersRepository>().As<IContentStorageUsersRepository>();

            /////container.RegisterTypedFactory<IContentStorageFactory>().ForConcreteType<IContentStorage>(); use delegate
            builder.RegisterType<ContentStorage>().As<IContentStorage>();


            /////container.RegisterTypedFactory<IDocumentUploadInfoFactory>().ForConcreteType<IDocumentUploadInfo>(); use delegate
            builder.RegisterType<DocumentUploadInfo>().As<IDocumentUploadInfo>();

            //container.RegisterTypedFactory<IDocumentMultiPartUploadInfoFactory>().ForConcreteType<IDocumentMultiPartUploadInfo>(); use delegate
            builder.RegisterType<DocumentMultiPartUploadInfo>().As<IDocumentMultiPartUploadInfo>();

            //container.RegisterTypedFactory<IDocumentIDUploadInfoFactory>().ForConcreteType<IDocumentIDUploadInfo>(); use delegate
            builder.RegisterType<DocumentIDUploadInfo>().As<IDocumentIDUploadInfo>();

            //container.RegisterTypedFactory<IDocumentMultiPartIDUploadInfoFactory>().ForConcreteType<IDocumentMultiPartIDUploadInfo>(); use delegate
            builder.RegisterType<DocumentMultiPartIDUploadInfo>().As<IDocumentMultiPartIDUploadInfo>();

            //container.RegisterTypedFactory<IUploadInfoFactory>().ForConcreteType<IUploadInfo>(); use delegate
            builder.RegisterType<UploadInfo>().As<IUploadInfo>();

            //container.RegisterTypedFactory<IIDUploadInfoFactory>().ForConcreteType<IIDUploadInfo>(); use delegate
            builder.RegisterType<IDUploadInfo>().As<IIDUploadInfo>();


            //container.RegisterTypedFactory<IContentStorageAccountFactory>().ForConcreteType<IContentStorageAccount>(); use delegate
            builder.RegisterType<ContentStorageAccount>().As<IContentStorageAccount>();

            //container.RegisterTypedFactory<IContentStorageIdentityFactory>().ForConcreteType<IContentStorageIdentity>(); use delegate
            builder.RegisterType<ContentStorageIdentity>().As<IContentStorageIdentity>();

            //container.RegisterTypedFactory<IContentStoragePrincipalFactory>().ForConcreteType<IContentStoragePrincipal>(); use delegate
            builder.RegisterType<ContentStoragePrincipal>().As<IContentStoragePrincipal>();

            //container.RegisterTypedFactory<IUserFactory>().ForConcreteType<IUser>(); use delegate
            builder.RegisterType<User>().As<IUser>();

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
