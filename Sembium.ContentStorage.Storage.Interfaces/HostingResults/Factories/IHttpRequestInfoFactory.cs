using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults.Factories
{
    public delegate IHttpRequestInfo IHttpRequestInfoFactory(string url, string method, IEnumerable<KeyValuePair<string, string>> headers);
}
