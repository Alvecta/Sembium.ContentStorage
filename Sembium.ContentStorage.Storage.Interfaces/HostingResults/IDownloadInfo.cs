using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public interface IDownloadInfo
    {
        string Url { get; }
        long Size { get; }
    }
}
