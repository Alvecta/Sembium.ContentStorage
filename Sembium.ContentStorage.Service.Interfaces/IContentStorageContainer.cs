using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.ServiceResults;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public interface IContentStorageContainer
    {
        IDocumentUploadInfo GetUploadInfoOrDocumentIdentifier(string hash, string extention);
        IDocumentIDUploadInfo GetIDUploadInfoOrDocumentID(string hash, string extention);

        IUploadInfo GetUploadInfo(IContentIdentifier contentIdentifier);
        IIDUploadInfo GetIDUploadInfo(string contentID);

        Task<IDocumentIdentifier> CommitUploadAsync(IUploadIdentifier uploadIdentifier);
        Task<string> CommitUploadAsync(string uploadID);

        string GetDocumentDownloadURL(IDocumentIdentifier documentIdentifier);
        string GetDocumentDownloadURL(string documentID);

        string GetContentDownloadURL(IContentIdentifier contentIdentifier);
        string GetContentDownloadURL(string contentID);

        IEnumerable<IContentIdentifier> GetContentIdentifiers();
        IEnumerable<string> GetContentIDs(DateTimeOffset afterMoment);
        string GetContentsHash(DateTimeOffset beforeMoment);

        void AddReadOnlySubcontainer(string subcontainerName);
        void RemoveReadOnlySubcontainer(string subcontainerName);
        IEnumerable<string> GetReadOnlySubcontainerNames();

        // multipart upload routines

        IDocumentMultiPartUploadInfo GetMultiPartUploadInfoOrDocumentIdentifier(string hash, string extention, long size);
        IDocumentMultiPartIDUploadInfo GetMultiPartIDUploadInfoOrDocumentID(string hash, string extention, long size);

        IMultiPartUploadInfo GetMultiPartUploadInfo(IContentIdentifier contentIdentifier, long size);
        IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(string contentID, long size);

        IDownloadInfo GetDocumentDownloadInfo(IDocumentIdentifier documentIdentifier);
        IDownloadInfo GetDocumentDownloadInfo(string documentID);

        IDownloadInfo GetContentDownloadInfo(IContentIdentifier contentIdentifier);
        IDownloadInfo GetContentDownloadInfo(string contentID);

        Task<IDocumentIdentifier> CommitMultiPartUploadAsync(IUploadIdentifier uploadIdentifier, IEnumerable<KeyValuePair<string, string>> partUploadResults);
        Task<string> CommitMultiPartUploadAsync(string uploadID, IEnumerable<KeyValuePair<string, string>> partUploadResults);

        Task<string> MaintainAsync(CancellationToken cancellationToken);
        Task CompactContentNamesAsync(CancellationToken cancellationToken);
    }
}
