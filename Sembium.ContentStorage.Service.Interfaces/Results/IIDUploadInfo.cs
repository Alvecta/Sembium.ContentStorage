using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public interface IIDUploadInfo
    {
        string URL { get; }
        string HttpMethod { get; }
        IEnumerable<KeyValuePair<string, string>> HttpHeaders { get; }
        string UploadID { get; }
    }
}
