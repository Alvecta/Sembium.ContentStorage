using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public class HttpRequestInfo : IHttpRequestInfo
    {
        public string URL { get; }
        public string Method { get;  }
        public IEnumerable<KeyValuePair<string, string>> Headers { get; }

        private HttpRequestInfo()
        {
            // do nothing
        }

        public HttpRequestInfo(string url, string method, IEnumerable<KeyValuePair<string, string>> headers)
        {
            URL = url;
            Method = method;
            Headers = headers;
        }
    }
}
