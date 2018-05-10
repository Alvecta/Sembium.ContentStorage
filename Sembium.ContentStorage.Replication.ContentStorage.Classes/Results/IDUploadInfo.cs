using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Results
{
    public class IDUploadInfo
    {
        public string URL { get; }
        public string HttpMethod { get; }
        public IEnumerable<KeyValuePair<string, string>> HttpHeaders { get; }
        public string UploadID { get; }

        public IDUploadInfo(string url, string httpMethod, IEnumerable<KeyValuePair<string, string>> httpHeaders, string uploadID)
        {
            URL = url;
            HttpMethod = httpMethod;
            HttpHeaders = httpHeaders;
            UploadID = uploadID;
        }
    }
}
