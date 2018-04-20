using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentNamesVaultItem
    {
        string Name { get; }
        Stream OpenReadStream();
        void LoadFromStream(Stream stream);
        bool CanAppend(bool compacting);
        void Append(Stream stream);
        Task DeleteAsync(CancellationToken cancellationToken);
    }
}
