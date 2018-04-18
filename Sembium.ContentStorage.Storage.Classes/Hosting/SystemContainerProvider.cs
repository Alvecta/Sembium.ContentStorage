using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public class SystemContainerProvider : ISystemContainerProvider
    {
        private const string SystemContainerName = "system";

        private readonly IContentStorageHost _contentStorageHost;

        public SystemContainerProvider(
            IContentStorageHost contentStorageHost)
        {
            _contentStorageHost = contentStorageHost;
        }

        public ISystemContainer GetSystemContainer(string specificName = null)
        {
            var containerName = string.IsNullOrEmpty(specificName) ? SystemContainerName : SystemContainerName + "-" + specificName;

            return _contentStorageHost.GetContainer(containerName, true) as ISystemContainer;
        }
    }
}
