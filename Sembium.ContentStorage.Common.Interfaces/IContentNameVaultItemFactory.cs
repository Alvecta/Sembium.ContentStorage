using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Common
{
    public delegate IContentNamesVaultItem IContentNameVaultItemFactory(IContent content, string name, bool isNew);
}
