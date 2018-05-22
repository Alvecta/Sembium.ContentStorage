using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public class CachingSystemContainerProvider : ISystemContainerProvider
    {
        private const string SystemContainerName = "system";

        private readonly ISystemContainerProvider _systemContainerProvider;
        private IDictionary<string, ISystemContainer> _cache;

        public CachingSystemContainerProvider(ISystemContainerProvider systemContainerProvider)
        {
            _systemContainerProvider = systemContainerProvider;

            _cache = new Dictionary<string, ISystemContainer>();
        }

        public ISystemContainer GetSystemContainer(string specificName = null)
        {
            var containerName = string.IsNullOrEmpty(specificName) ? SystemContainerName : SystemContainerName + "-" + specificName;

            if (_cache.TryGetValue(containerName, out var existing))
            {
                return existing;
            }

            var result = _systemContainerProvider.GetSystemContainer(specificName);

            _cache.Add(containerName, result);

            return result;
        }
    }
}
