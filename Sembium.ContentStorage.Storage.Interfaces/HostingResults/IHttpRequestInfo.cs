using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public interface IHttpRequestInfo
    {
        string URL { get; }
        string Method { get;  }
        IEnumerable<KeyValuePair<string, string>> Headers { get; }
    }
}
