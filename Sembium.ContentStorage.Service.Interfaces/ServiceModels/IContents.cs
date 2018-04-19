using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public interface IContents
    {
        IDocumentIDUploadInfo GetIDUploadInfoOrDocumentID(string containerName, string hash, string extention, string authenticationToken);
        IIDUploadInfo GetIDUploadInfo(string containerName, string contentID, string authenticationToken);
        
        Task<string> CommitUploadAsync(string containerName, string uploadID, string authenticationToken);

        string GetDocumentDownloadUrl(string containerName, string documentID, string authenticationToken);
        string GetContentDownloadUrl(string containerName, string contentID, string authenticationToken);

        IEnumerable<string> GetContentIDs(string containerName, DateTimeOffset afterMoment, string authenticationToken);
        int GetContentCount(string containerName, string authenticationToken);
        string GetContentsHash(string containerName, DateTimeOffset beforeMoment, string authenticationToken);

        // multipart upload routines

        IDocumentMultiPartIDUploadInfo GetMultiPartIDUploadInfoOrDocumentID(string containerName, string hash, string extention, long size, string authenticationToken);
        IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(string containerName, string contentID, long size, string authenticationToken);

        Task<string> CommitMultiPartUploadAsync(string containerName, string uploadID, IEnumerable<KeyValuePair<string, string>> partUploadResults, string authenticationToken);

        IDownloadInfo GetDocumentDownloadInfo(string containerName, string documentID, string authenticationToken);
        IDownloadInfo GetContentDownloadInfo(string containerName, string contentID, string authenticationToken);

        IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken);
    }
}
