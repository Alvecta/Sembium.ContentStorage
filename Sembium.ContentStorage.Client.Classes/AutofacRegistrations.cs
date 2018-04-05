using Autofac;

namespace Sembium.ContentStorage.Client
{
    public static class AutofacRegistrations
    {
        public static void RegisterFor(ContainerBuilder builder)
        {
            builder.RegisterType<ContentStorageServiceURLProvider>().As<IContentStorageServiceURLProvider>();
            builder.RegisterType<ContentStorageUploader>().As<IContentStorageUploader>();
        }
    }
}
