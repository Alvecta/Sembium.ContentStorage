using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Service.Hosting;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public class ContainerStateRepository : IContainerStateRepository
    {
        private const string ContainerStatesContentName = "containerstates.json";

        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IContainerStateFactory _containerStateFactory;
        private readonly ISerializer _serializer;

        public ContainerStateRepository(
            IContentIdentifierGenerator contentIdentifierGenerator,
            ISystemContainerProvider systemContainerProvider,
            IContainerStateFactory containerStateFactory,
            ISerializer serializer)
        {
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _systemContainerProvider = systemContainerProvider;
            _containerStateFactory = containerStateFactory;
            _serializer = serializer;
        }

        public async Task<IContainerState> GetContainerStateAsync(string containerName)
        {
            var result =
                LoadContainerStates()
                .Where(x => x.ContainerName.Equals(containerName, StringComparison.InvariantCultureIgnoreCase))
                .SingleOrDefault();

            return await Task.FromResult(result);
        }

        public IEnumerable<IContainerState> GetContainerStates()
        {
            return LoadContainerStates();
        }

        public async Task SetContainerStateAsync(string containerName, bool? isReadOnly, bool? isMaintained)
        {
            var oldContainerState = await GetContainerStateAsync(containerName);

            var newContainerState = _containerStateFactory(containerName, isReadOnly ?? oldContainerState?.IsReadOnly ?? false, isMaintained ?? oldContainerState?.IsMaintained ?? false);

            var newContainerStates =
                LoadContainerStates()
                .Where(x => !x.ContainerName.Equals(containerName, StringComparison.InvariantCultureIgnoreCase))
                .Union(new[] { newContainerState });

            await SaveContainerStatesAsync(newContainerStates);
        }

        private IEnumerable<IContainerState> LoadContainerStates()
        {
            var containerStatesContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(ContainerStatesContentName);
            var serializedContainerStatesContainers = _systemContainerProvider.GetSystemContainer().GetStringContent(containerStatesContentIdentifier);

            if (string.IsNullOrEmpty(serializedContainerStatesContainers))
            {
                return Enumerable.Empty<IContainerState>();
            }

            return _serializer.Deserialize<IEnumerable<IContainerState>>(serializedContainerStatesContainers);
        }

        private async Task SaveContainerStatesAsync(IEnumerable<IContainerState> containerStates)
        {
            var serializedContainerStates = _serializer.Serialize(containerStates);

            var containerStatesContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(ContainerStatesContentName);

            _systemContainerProvider.GetSystemContainer().SetStringContent(containerStatesContentIdentifier, serializedContainerStates);

            await Task.CompletedTask;
        }
    }
}
