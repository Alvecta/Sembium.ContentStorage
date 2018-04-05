using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class HttpPartUploadInfo : IHttpPartUploadInfo
    {
        public string Identifier { get; private set; }
        public string URL { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> RequestHttpHeaders { get; private set; }

        public HttpPartUploadInfo(string identifier, string url, IEnumerable<KeyValuePair<string, string>> requestHttpHeaders)
        {
            Identifier = identifier;
            URL = url;
            RequestHttpHeaders = requestHttpHeaders;
        }
    }
}
