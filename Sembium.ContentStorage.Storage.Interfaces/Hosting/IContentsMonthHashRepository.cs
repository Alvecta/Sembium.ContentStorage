using Sembium.ContentStorage.Storage.HostingResults;
using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IContentsMonthHashRepository
    {
        IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName);
        void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts);
    }
}
