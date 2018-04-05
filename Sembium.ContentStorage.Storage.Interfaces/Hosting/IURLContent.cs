using Sembium.ContentStorage.Storage.HostingResults;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IURLContent : IContent
    {
        IMultiPartUploadInfo GetMultiPartUploadInfo(int expirySeconds, long size);
        IDownloadInfo GetDownloadInfo(int expirySeconds);
    }
}
