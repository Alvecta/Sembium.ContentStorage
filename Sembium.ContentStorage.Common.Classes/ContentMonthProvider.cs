using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class ContentMonthProvider : IContentMonthProvider
    {
        public DateTimeOffset GetContentMonth(DateTimeOffset contentDate)
        {
            return new DateTimeOffset(contentDate.ToUniversalTime().Year, contentDate.ToUniversalTime().Month, 1, 0, 0, 0, TimeSpan.FromHours(0));
        }
    }
}
