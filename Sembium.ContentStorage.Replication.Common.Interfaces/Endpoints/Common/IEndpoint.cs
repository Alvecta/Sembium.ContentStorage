using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Common
{
    public interface IEndpoint
    {
        string ID { get; }
        Task<IEnumerable<IContentIdentifier>> GetContentIdentifiersAsync(DateTimeOffset afterMoment);
        Task<string> GetContentsHashAsync(DateTimeOffset beforeMoment);
    }
}
