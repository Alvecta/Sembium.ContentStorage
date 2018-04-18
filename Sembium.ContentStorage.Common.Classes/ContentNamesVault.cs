using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Common
{
    public class ContentNamesVault : IContentNamesVault
    {
        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IContentNameVaultItemFactory _contentNameVaultItemFactory;

        public ContentNamesVault(
            ISystemContainerProvider systemContainerProvider,
            IContentNameVaultItemFactory contentNameVaultItemFactory)
        {
            _systemContainerProvider = systemContainerProvider;
            _contentNameVaultItemFactory = contentNameVaultItemFactory;
        }

        public IEnumerable<IContentNamesVaultItem> GetItems(string contentsContainerName, string prefix)
        {
            var namesContainer = GetNamesContainer();
            var contents = namesContainer.GetContents(contentsContainerName + "/" + prefix);

            return contents.Select(x => _contentNameVaultItemFactory(x, x.SimpleName.Split('/').Last(), false));
        }

        public IContentNamesVaultItem GetNewItem(string contentsContainerName, string name)
        {
            var namesContainer = GetNamesContainer();
            var content = namesContainer.GetContent(contentsContainerName + "/" + name);

            return _contentNameVaultItemFactory(content, name, true);
        }

        private ISystemContainer GetNamesContainer()
        {
            return _systemContainerProvider.GetSystemContainer("names");
        }
    }
}
