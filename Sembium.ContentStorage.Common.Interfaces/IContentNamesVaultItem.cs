using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentNamesVaultItem
    {
        string Name { get; }
        Stream OpenReadStream();
        bool CanAppend();
        void Append(Stream stream);
    }
}
