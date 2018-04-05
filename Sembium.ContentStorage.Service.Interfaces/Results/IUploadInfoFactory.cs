using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public delegate IUploadInfo IUploadInfoFactory(string url, string httpMethod, IEnumerable<KeyValuePair<string, string>> httpHeaders, IUploadIdentifier uploadIdentifier);
}
