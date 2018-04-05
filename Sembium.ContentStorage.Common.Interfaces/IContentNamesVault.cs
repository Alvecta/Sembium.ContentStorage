using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentNamesVault
    {
        IEnumerable<IContentNamesVaultItem> GetItems(string contentsContainerName, string prefix);
        IContentNamesVaultItem GetNewItem(string contentsContainerName, string name);
    }
}
