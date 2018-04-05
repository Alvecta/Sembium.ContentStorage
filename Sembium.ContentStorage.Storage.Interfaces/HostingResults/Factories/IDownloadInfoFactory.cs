using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults.Factories
{
    public delegate IDownloadInfo IDownloadInfoFactory(string url, long size);
}
