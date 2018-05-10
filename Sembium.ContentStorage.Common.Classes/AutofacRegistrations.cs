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
            builder.RegisterType<ConfigurationSettings>().As<IConfigurationSettings>();
            builder.RegisterType<JavaScriptSerializer>().As<ISerializer>();
            builder.RegisterType<HashStringProvider>().As<IHashStringProvider>();
            builder.RegisterType<Sha1HashProvider>().As<IHashProvider>();
            builder.RegisterType<AutofacContractResolver>().As<Newtonsoft.Json.Serialization.IContractResolver>();
        }
    }
}
