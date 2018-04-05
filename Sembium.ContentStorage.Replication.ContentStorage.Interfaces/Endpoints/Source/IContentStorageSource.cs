using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Source
{
    public interface IContentStorageSource : ISource
    {
        IDownloadInfo GetDownloadInfo(IContentIdentifier contentIdentifier);
    }
}
