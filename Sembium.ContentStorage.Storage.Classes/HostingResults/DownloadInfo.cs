using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class DownloadInfo : IDownloadInfo
    {
        public string Url { get; private set; }

        public long Size { get; private set; }

        public DownloadInfo(string url, long size)
        {
            Url = url;
            Size = size;
        }
    }
}
