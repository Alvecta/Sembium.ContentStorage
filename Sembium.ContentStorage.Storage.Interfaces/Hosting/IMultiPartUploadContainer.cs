using System.Collections.Generic;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IMultiPartUploadContainer
    {
       void FinalizeMultiPartUpload(string hostIdentifier, IEnumerable<KeyValuePair<string, string>> partUploadResults);
    }
}
