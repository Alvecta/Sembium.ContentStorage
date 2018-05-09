using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Client
{
    public interface IContentStorageServiceURLProvider
    {
        string GetURLForGetContentIdentifiers(string serviceURL, string containerName, DateTimeOffset? afterMoment, int? maxCount, string afterContentID, string authenticationToken);
        string GetURLForGetContentsHash(string serviceURL, string containerName, DateTimeOffset beforeMoment, string authenticationToken);
        string GetURLForGetContentDownloadURL(string serviceURL, string containerName, string contentID, string authenticationToken);
        string GetURLForGetUploadInfo(string serviceURL, string containerName, string contentID, string authenticationToken);
        string GetURLForCommitUpload(string serviceURL, string containerName, string uploadID, string authenticationToken);
        
        // multipart upload
        string GetURLForGetContentDownloadInfo(string serviceURL, string containerName, string contentID, string authenticationToken);
        string GetURLForGetMultiPartUploadInfo(string serviceURL, string containerName, string contentID, long size, string authenticationToken);
        string GetURLForCommitMultiPartUpload(string serviceURL, string containerName, string authenticationToken);

        string GetURLForGetUrlContentUploadInfo(string serviceURL, string containerName, string contentID, long size, string authenticationToken);
    }
}
