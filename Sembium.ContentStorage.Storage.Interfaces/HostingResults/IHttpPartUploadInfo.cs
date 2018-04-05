using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.HostingResults
{
    public interface IHttpPartUploadInfo
    {
        string Identifier { get; }
        string URL { get; }
        IEnumerable<KeyValuePair<string, string>> RequestHttpHeaders { get; }
    }
}
