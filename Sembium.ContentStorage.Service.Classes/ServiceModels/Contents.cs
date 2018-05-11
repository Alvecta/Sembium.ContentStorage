using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public class Contents : IContents
    {
        private readonly IContentStorageAccountProvider _contentStorageAccountProvider;

        public Contents(
            IContentStorageAccountProvider contentStorageAccountProvider)
        {
            _contentStorageAccountProvider = contentStorageAccountProvider;
        }

        public IDocumentIDUploadInfo GetIDUploadInfoOrDocumentID(string containerName, string hash, string extention, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetIDUploadInfoOrDocumentID(hash, extention);
        }

        public IIDUploadInfo GetIDUploadInfo(string containerName, string contentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetIDUploadInfo(contentID);
        }

        public async Task<string> CommitUploadAsync(string containerName, string uploadID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return await contentStorageContainer.CommitUploadAsync(uploadID);
        }
        public string GetDocumentDownloadUrl(string containerName, string documentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetDocumentDownloadURL(documentID);
        }

        public string GetContentDownloadUrl(string containerName, string contentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetContentDownloadURL(contentID);
        }

        public IEnumerable<string> GetContentIDs(string containerName, DateTimeOffset? afterMoment, int? maxCount, string afterContentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetContentIDs(afterMoment, maxCount, afterContentID);
        }

        public int GetContentCount(string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetContentIdentifiers().Count();
        }

        public string GetContentsHash(string containerName, DateTimeOffset beforeMoment, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetContentsHash(beforeMoment);
        }

        public IDocumentMultiPartIDUploadInfo GetMultiPartIDUploadInfoOrDocumentID(string containerName, string hash, string extention, long size, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetMultiPartIDUploadInfoOrDocumentID(hash, extention, size);
        }

        public IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(string containerName, string contentID, long size, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetMultiPartIDUploadInfo(contentID, size);
        }

        public Task<string> CommitMultiPartUploadAsync(string containerName, string uploadID, IEnumerable<KeyValuePair<string, string>> partUploadResults, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.CommitMultiPartUploadAsync(uploadID, partUploadResults);
        }

        public IDownloadInfo GetDocumentDownloadInfo(string containerName, string documentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetDocumentDownloadInfo(documentID);
        }

        public IDownloadInfo GetContentDownloadInfo(string containerName, string contentID, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorageContainer = account.GetContentStorageContainer();

            return contentStorageContainer.GetContentDownloadInfo(contentID);
        }

        public IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, containerName);
            var contentStorage = account.GetContentStorage();

            return contentStorage.GetUrlContentUploadInfo(contentUrl, contentStorageServiceUrl, containerName, contentID, size, authenticationToken);
        }
    }
}