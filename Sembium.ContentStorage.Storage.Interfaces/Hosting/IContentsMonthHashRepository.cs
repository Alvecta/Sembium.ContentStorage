using Sembium.ContentStorage.Storage.HostingResults;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IContentsMonthHashRepository
    {
        IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName);
        void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts);
        Task CompactAsync(string containerName, CancellationToken cancellationToken);
    }
}
