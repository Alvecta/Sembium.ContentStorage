using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class IDUploadInfo : IIDUploadInfo
    {
        public string URL { get; private set; }
        public string HttpMethod { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> HttpHeaders { get; private set; }
        public string UploadID { get; private set; }

        public IDUploadInfo(string url, string httpMethod, IEnumerable<KeyValuePair<string, string>> httpHeaders, string uploadID)
        {
            URL = url;
            HttpMethod = httpMethod;
            HttpHeaders = httpHeaders;
            UploadID = uploadID;
        }
    }
}
