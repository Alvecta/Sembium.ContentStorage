using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentNamesRepository
    {
        void AddContent(string containerName, string contentName, DateTimeOffset contentDate, CancellationToken cancellationToken);
        int AddContents(string containerName, IEnumerable<KeyValuePair<string, DateTimeOffset>> contents, CancellationToken cancellationToken);
        IEnumerable<string> GetChronologicallyOrderedContentNames(string containerName, DateTimeOffset? beforeMonth, DateTimeOffset? afterMonth, CancellationToken cancellationToken);
        Task CompactAsync(string containerName, CancellationToken cancellationToken);
    }
}
