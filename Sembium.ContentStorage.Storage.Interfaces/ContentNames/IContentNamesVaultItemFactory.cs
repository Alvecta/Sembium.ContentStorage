using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.ContentNames
{
    public delegate IContentNamesVaultItem IContentNamesVaultItemFactory(IContent content, string name, bool isNew);
}

