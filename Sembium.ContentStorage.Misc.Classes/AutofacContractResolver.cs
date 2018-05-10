using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Misc
{
    // copied from https://www.newtonsoft.com/json/help/html/DeserializeWithDependencyInjection.htm
    // and changed to use IComponentContext instead of IContainer
    public class AutofacContractResolver : DefaultContractResolver
    {
        private readonly IComponentContext _componentContext;

        public AutofacContractResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            // use Autofac to create types that have been registered with it
            if (_componentContext.IsRegistered(objectType))
            {
                JsonObjectContract contract = ResolveContact(objectType);
                contract.DefaultCreator = () => _componentContext.Resolve(objectType);

                return contract;
            }

            return base.CreateObjectContract(objectType);
        }

        private JsonObjectContract ResolveContact(Type objectType)
        {
            // attempt to create the contact from the resolved type
            IComponentRegistration registration;
            if (_componentContext.ComponentRegistry.TryGetRegistration(new TypedService(objectType), out registration))
            {
                Type viewType = (registration.Activator as ReflectionActivator)?.LimitType;
                if (viewType != null)
                {
                    return base.CreateObjectContract(viewType);
                }
            }

            // fall back to using the registered type
            return base.CreateObjectContract(objectType);
        }
    }
}
