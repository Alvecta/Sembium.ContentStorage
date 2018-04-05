using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public static class HashProviderExtensions
    {
        public static (byte[] Hash, int Count) GetHashAndCount(this IHashProvider hashProvider, IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            monthHashAndCounts = monthHashAndCounts.ToList();  // enumerate once

            var hashResult = hashProvider.GetHashAndCount(monthHashAndCounts.Select(x => x.Hash));

            return (hashResult.Hash, monthHashAndCounts.Sum(x => x.Count));
        }
    }
}
