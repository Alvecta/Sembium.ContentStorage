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
        public string URL { get; }
        public string HttpMethod { get; }
        public IEnumerable<KeyValuePair<string, string>> HttpHeaders { get; }
        public IUploadIdentifier UploadIdentifier { get; }

        public UploadInfo(string url, string httpMethod, IEnumerable<KeyValuePair<string, string>> httpHeaders, IUploadIdentifier uploadIdentifier)
        {
            URL = url;
            HttpMethod = httpMethod;
            HttpHeaders = httpHeaders;
            UploadIdentifier = uploadIdentifier;
        }
    }
}
