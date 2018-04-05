using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class UploadInfo : IUploadInfo
    {
        public string URL { get; private set; }
        public string HttpMethod { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> HttpHeaders { get; private set; }
        public IUploadIdentifier UploadIdentifier { get; private set; }

        public UploadInfo(string url, string httpMethod, IEnumerable<KeyValuePair<string, string>> httpHeaders, IUploadIdentifier uploadIdentifier)
        {
            URL = url;
            HttpMethod = httpMethod;
            HttpHeaders = httpHeaders;
            UploadIdentifier = uploadIdentifier;
        }
    }
}
