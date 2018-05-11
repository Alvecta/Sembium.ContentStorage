using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Common.ContentNames.Vault
{
    public class ContentNamesVault : IContentNamesVault
    {
        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IContentNamesVaultItemFactory _contentNameVaultItemFactory;

        public ContentNamesVault(
            ISystemContainerProvider systemContainerProvider,
            IContentNamesVaultItemFactory contentNameVaultItemFactory)
        {
            _systemContainerProvider = systemContainerProvider;
            _contentNameVaultItemFactory = contentNameVaultItemFactory;
        }

        public IEnumerable<IContentNamesVaultItem> GetItems(string contentsContainerName, string prefix)
        {
            var namesContainer = GetNamesContainer(contentsContainerName);
            var contents = namesContainer.GetContents(prefix);

            return contents.Select(x => _contentNameVaultItemFactory(x, x.SimpleName, false));
        }

        public IContentNamesVaultItem GetNewItem(string contentsContainerName, string name)
        {
            var namesContainer = GetNamesContainer(contentsContainerName);
            var content = namesContainer.GetContent(name);

            return _contentNameVaultItemFactory(content, name, true);
        }

        private ISystemContainer GetNamesContainer(string contentsContainerName)
        {
            return _systemContainerProvider.GetSystemContainer("names/" + contentsContainerName);
        }
    }
}
