using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Client
{
    public class ContentStorageServiceURLProvider : IContentStorageServiceURLProvider
    {
        public string GetURLForGetContentIdentifiers(string serviceURL, string containerName, DateTimeOffset? afterMoment, int? maxCount, string afterContentID, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/containers/{0}?auth={1}", containerName, authenticationToken) +
                (afterMoment.HasValue ? "&after=" + afterMoment.Value.ToString("u") : null) +
                (maxCount.HasValue ? "&maxCount=" + maxCount.Value.ToString() : null) +
                (string.IsNullOrEmpty(afterContentID) ? null: "&afterContentID=" + WebUtility.UrlEncode(afterContentID));
        }

        public string GetURLForGetContentsHash(string serviceURL, string containerName, DateTimeOffset beforeMoment, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/containers/{0}/hash?before={1}&auth={2}", containerName, beforeMoment.ToString("u"), authenticationToken);
        }

        public string GetURLForGetContentDownloadURL(string serviceURL, string containerName, string contentID, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/GetContentDownloadUrl/{0}?contentID={1}&auth={2}", containerName, WebUtility.UrlEncode(contentID), authenticationToken);
        }

        public string GetURLForGetUploadInfo(string serviceURL, string containerName, string contentID, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/GetUploadInfo/{0}?contentID={1}&auth={2}", containerName, WebUtility.UrlEncode(contentID), authenticationToken);

        }

        public string GetURLForCommitUpload(string serviceURL, string containerName, string uploadID, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/CommitUpload/{0}?uploadID={1}&auth={2}", containerName, WebUtility.UrlEncode(uploadID), authenticationToken);
        }

        public string GetURLForGetContentDownloadInfo(string serviceURL, string containerName, string contentID, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/GetContentDownloadInfo/{0}?contentID={1}&auth={2}", containerName, WebUtility.UrlEncode(contentID), authenticationToken);
        }

        public string GetURLForGetMultiPartUploadInfo(string serviceURL, string containerName, string contentID, long size, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/GetMultiPartUploadInfo/{0}?contentID={1}&size={2}&auth={3}", containerName, WebUtility.UrlEncode(contentID), size, authenticationToken);
        }

        public string GetURLForCommitMultiPartUpload(string serviceURL, string containerName, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                string.Format("/contents/CommitMultiPartUpload/{0}?auth={1}", containerName, authenticationToken);
        }

        public string GetURLForGetUrlContentUploadInfo(string serviceURL, string containerName, string contentID, long size, string authenticationToken)
        {
            return
                serviceURL.TrimEnd('/') +
                $"/contents/GetUrlContentUploadInfo/{containerName}?contentID={WebUtility.UrlEncode(contentID)}&size={size}&auth={authenticationToken}";
        }
    }
}
