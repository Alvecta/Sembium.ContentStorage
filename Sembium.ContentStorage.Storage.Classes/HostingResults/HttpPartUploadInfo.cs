using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class HttpPartUploadInfo : IHttpPartUploadInfo
    {
        public string Identifier { get; }
        public string URL { get; }
        public IEnumerable<KeyValuePair<string, string>> RequestHttpHeaders { get; }

        private HttpPartUploadInfo()
        {
            // do nothing
        }

        public HttpPartUploadInfo(string identifier, string url, IEnumerable<KeyValuePair<string, string>> requestHttpHeaders)
        {
            Identifier = identifier;
            URL = url;
            RequestHttpHeaders = requestHttpHeaders;
        }
    }
}
