using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common.MonthHash
{
    public delegate IMonthHashAndCount IMonthHashAndCountFactory(DateTimeOffset month, byte[] hash, int count, DateTimeOffset? lastModifiedMoment);
}
