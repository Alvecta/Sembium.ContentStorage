using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentsMonthHashRepository
    {
        IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName);
        void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts);
        Task CompactAsync(string containerName, CancellationToken cancellationToken);
    }
}
