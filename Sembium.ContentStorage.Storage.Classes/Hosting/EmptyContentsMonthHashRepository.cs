using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sembium.ContentStorage.Storage.HostingResults;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public class EmptyContentsMonthHashRepository : IContentsMonthHashRepository
    {
        public void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            // do nothing to keep the repository empty
        }

        public IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName)
        {
            return Enumerable.Empty<IMonthHashAndCount>();
        }
    }
}
