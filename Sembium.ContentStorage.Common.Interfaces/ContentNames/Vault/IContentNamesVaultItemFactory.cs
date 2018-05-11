using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Common.ContentNames.Vault
{
    public delegate IContentNamesVaultItem IContentNamesVaultItemFactory(IContent content, string name, bool isNew);
}

