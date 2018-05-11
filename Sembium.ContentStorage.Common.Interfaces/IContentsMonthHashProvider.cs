using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IContentsMonthHashProvider
    {
        IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(IEnumerable<IContentIdentifier> contentIdentifiers);
    }
}
