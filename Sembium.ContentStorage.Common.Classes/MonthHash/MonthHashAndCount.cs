using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common.MonthHash
{
    public class MonthHashAndCount : IMonthHashAndCount
    {
        public DateTimeOffset Month { get; }
        public byte[] Hash { get; }
        public int Count { get; }
        public DateTimeOffset? LastModifiedMoment { get; }

        public MonthHashAndCount(DateTimeOffset month, byte[] hash, int count, DateTimeOffset? lastModifiedMoment)
        {
            Month = month;
            Hash = hash;
            Count = count;
            LastModifiedMoment = lastModifiedMoment;
        }
    }
}
