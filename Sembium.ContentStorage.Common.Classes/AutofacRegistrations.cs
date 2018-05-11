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
        }
    }
}
