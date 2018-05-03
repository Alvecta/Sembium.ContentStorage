using Autofac;
using Autofac.Builder;
using Autofac.Features.LightweightAdapters;
using Microsoft.Extensions.Options;
using System;

namespace Sembium.ContentStorage.Utils.Autofac
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterOptionsAdapter<TTo>(this ContainerBuilder builder) where TTo : class, new()
        {
            return builder.RegisterAdapter<IOptions<TTo>, TTo>(OptionsAdapter.Adapt);
        }
    }
}
