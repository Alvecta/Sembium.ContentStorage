using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<CertificateStoreCertificateLoader>().Named<ICertificateLoader>("CertificateStoreCertificateLoader").As<ICertificateLoader>();
            builder.RegisterType<FileSystemCertificateLoader>().Named<ICertificateLoader>("FileSystemCertificateLoader").As<ICertificateLoader>();

            builder.RegisterType<CertificateProvider>().Named<ICertificateProvider>("base");
            builder.RegisterType<CachedCertificateProvider>().Named<ICertificateProvider>("CachedCertificateProvider");
            builder.RegisterDecorator<ICertificateProvider>((x, inner) => x.ResolveNamed<ICertificateProvider>("CachedCertificateProvider", TypedParameter.From(inner)), "base").As<ICertificateProvider>().SingleInstance();

            builder.RegisterType<SignatureProvider>().As<ISignatureProvider>();
            builder.RegisterType<SignedURLProvider>().As<ISignedURLProvider>();
        }
    }
}
