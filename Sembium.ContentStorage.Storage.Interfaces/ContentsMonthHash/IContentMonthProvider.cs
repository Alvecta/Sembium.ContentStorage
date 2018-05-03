using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.ContentsMonthHash
{
    public interface IContentMonthProvider
    {
        DateTimeOffset GetContentMonth(DateTimeOffset contentDate);
    }
}
