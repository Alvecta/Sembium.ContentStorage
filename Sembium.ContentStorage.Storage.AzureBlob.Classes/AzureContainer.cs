using Microsoft.WindowsAzure.Storage.Blob;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContainer : IContainer, IMultiPartUploadContainer
    {
        private const string AzureApiVersion = "2017-07-29";

        private readonly Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer _delegateContainer;
        private readonly IAzureContentFactory _azureContentFactory;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IContentHashValidator _contentHashValidator;
        private readonly IContentNamesRepository _committedContentNamesRepository;
        private readonly IContentMonthProvider _contentMonthProvider;
        private readonly IContentsMonthHashProvider _contentsMonthHashProvider;
        private readonly IHashProvider _hashProvider;
        private readonly IContentsMonthHashRepository _contentsMonthHashRepository;

        public AzureContainer(
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer delegateContainer, 
            IAzureContentFactory azureContentFactory,
            IContentNameProvider contentNameProvider,
            IContentIdentifierGenerator contentIdentifierGenerator,
            IContentHashValidator contentHashValidator,
            IContentNamesRepository committedContentNamesRepository,
            IContentMonthProvider contentMonthProvider,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IHashProvider hashProvider,
            IContentsMonthHashRepository contentsMonthHashRepository)
        {
            _delegateContainer = delegateContainer;
            _azureContentFactory = azureContentFactory;
            _contentNameProvider = contentNameProvider;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _contentHashValidator = contentHashValidator;
            _committedContentNamesRepository = committedContentNamesRepository;
            _contentMonthProvider = contentMonthProvider;
            _contentsMonthHashProvider = contentsMonthHashProvider;
            _hashProvider = hashProvider;
            _contentsMonthHashRepository = contentsMonthHashRepository;
        }

        private bool ContentExists(string contentName)
        {
            if (contentName == null)
                return false;

            var blockBlobReference = _delegateContainer.GetBlockBlobReference(contentName);
            return blockBlobReference.ExistsAsync().Result;
        }

        public bool ContentExists(IContentIdentifier contentIdentifier)
        {
            var contentName = _contentNameProvider.GetContentName(contentIdentifier);
            return ContentExists(contentName);
        }

        public IContent CreateContent(IContentIdentifier contentIdentifier)
        {
            if (ContentExists(contentIdentifier))
                throw new UserException("Content already exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);
            var blockBlobReference = _delegateContainer.GetBlockBlobReference(contentName);

            return _azureContentFactory(blockBlobReference);
        }

        public IContent GetContent(IContentIdentifier contentIdentifier)
        {
            if (!ContentExists(contentIdentifier))
                throw new UserException("Content does not exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);
            var blockBlobReference = _delegateContainer.GetBlockBlobReference(contentName);

            return _azureContentFactory(blockBlobReference);
        }

        public IEnumerable<IContentIdentifier> GetContentIdentifiers(bool committed, string hash)
        {
            var prefix = _contentNameProvider.GetSearchPrefix(hash);

            prefix = prefix?.ToLower();  // azure specific

            return
                GetAllContentIdentifiers(prefix)
                .Where(x => x.Uncommitted != committed);
        }

        public IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment)
        {
            var result =
                _committedContentNamesRepository.GetChronologicallyOrderedContentNames(_delegateContainer.Name, GetMomentMonth(beforeMoment, 1), GetMomentMonth(afterMoment, -1), CancellationToken.None)
                .Select(x => _contentNameProvider.GetContentIdentifier(x))
                .Where(x => x != null);

            if (beforeMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment < beforeMoment.Value);
            }

            if (afterMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment > afterMoment.Value);
            }

            return result;
        }

        private DateTimeOffset? GetMomentMonth(DateTimeOffset? moment, int monthsOffset = 0)
        {
            if (moment.HasValue)
            {
                return new DateTimeOffset(moment.Value.ToUniversalTime().Year, moment.Value.ToUniversalTime().Month, 1, 0, 0, 0, TimeSpan.FromHours(0)).AddMonths(monthsOffset);
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<string> GetAllContentNames(string prefix)
        {
            return GetAllBlobs(prefix).Select(x => x.Name);
        }

        private IEnumerable<CloudBlob> GetAllBlobs(string prefix)
        {
            BlobContinuationToken continuationToken = null;

            while (true)
            {
                var result = _delegateContainer.ListBlobsSegmentedAsync(prefix, true, BlobListingDetails.None, null, continuationToken, null, null).Result;

                var blobs = result.Results.OfType<CloudBlob>();

                foreach (var blob in blobs)
                {
                    yield return blob;
                }

                if (result.ContinuationToken == null)
                    break;

                continuationToken = result.ContinuationToken;
            }
        }

        private IEnumerable<IContentIdentifier> GetContentIdentifiers(IEnumerable<string> contentNames)
        {
            return
                contentNames
                .Select(x => _contentNameProvider.GetContentIdentifier(x))
                .Where(x => x != null);
        }


        private IEnumerable<IContentIdentifier> GetAllContentIdentifiers(string prefix)
        {
            return GetContentIdentifiers(GetAllContentNames(prefix));
        }

        public void FinalizeMultiPartUpload(string hostIdentifier, IEnumerable<KeyValuePair<string, string>> partUploadResults)
        {
            if ((partUploadResults == null) || (!partUploadResults.Any()))
            {
                return;
            }

            var partIdentifiers = partUploadResults.Select(x => x.Key);

            using (var client = GetHttpClient())
            {
                var contentBuilder = new StringBuilder();
                contentBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                contentBuilder.AppendLine("<BlockList>");

                foreach (var partIdentifier in partIdentifiers)
                {
                    contentBuilder.AppendFormat("<Uncommitted>{0}</Uncommitted>", partIdentifier);
                }
                contentBuilder.AppendLine("</BlockList>");
                var contentString = contentBuilder.ToString();

                HttpContent content = new StringContent(contentString);
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Plain);
                content.Headers.ContentLength = contentString.Length;

                var commitUrl = hostIdentifier + "&comp=blocklist";

                var result = client.PutAsync(commitUrl, content);

                ProcessResult(result);
            }
        }

        public async Task<IContentIdentifier> CommitContentAsync(IContentIdentifier uncommittedContentIdentifier)
        {
            System.Diagnostics.Debug.Assert(uncommittedContentIdentifier.Uncommitted);

            var uncommittedContent = GetContent(uncommittedContentIdentifier);
            _contentHashValidator.ValidateHash(uncommittedContent, uncommittedContentIdentifier.Hash);
            var uncommittedContentName = _contentNameProvider.GetContentName(uncommittedContentIdentifier);
            var contentIdentifier = _contentIdentifierGenerator.GetCommittedContentIdentifier(uncommittedContentIdentifier);
            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            await RenameContentAsync(uncommittedContentName, contentName);
            await PersistCommittedContentNameAsync(contentName, contentIdentifier.ModifiedMoment);

            return contentIdentifier;
        }

        private async Task PersistCommittedContentNameAsync(string contentName, DateTimeOffset contentDate)
        {
            await Task.Run(() =>
            {
                _committedContentNamesRepository.AddContent(_delegateContainer.Name, contentName.ToLowerInvariant(), contentDate, CancellationToken.None);
            });
        }

        private static HttpResponseMessage ProcessResult(Task<HttpResponseMessage> task)
        {
            HttpResponseMessage response = null;
            try
            {
                response = task.Result;
            }
            catch (Exception exception)
            {
                var innerExceptionMessage = exception?.InnerException?.Message ?? "No Inner Exception";

                var message = String.Format("Unable to finish request. Application Exception: {0}; With inner exception: {1}", exception.Message, innerExceptionMessage);
                throw new ApplicationException(message);
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var exceptionMessage = String.Format("Unable to finish request. Server returned status: {0}; {1}", response.StatusCode, response.ReasonPhrase);
            throw new ApplicationException(exceptionMessage);
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", DateTime.Now.ToUniversalTime().ToString("r"));
            client.DefaultRequestHeaders.Add("x-ms-version", AzureApiVersion);

            return client;
        }

        private async Task RenameContentAsync(string oldContentName, string newContentName)
        {
            var oldBlockBlobReference = _delegateContainer.GetBlockBlobReference(oldContentName);
            var newBlockBlobReference = _delegateContainer.GetBlockBlobReference(newContentName);

            await newBlockBlobReference.StartCopyAsync(oldBlockBlobReference);

            while (true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));

                var blobs = _delegateContainer.ListBlobsSegmentedAsync(newContentName, true, BlobListingDetails.Copy, 1, null, null, null);

                var blob = blobs.Result.Results.FirstOrDefault() as CloudBlob;

                if (blob != null)
                {
                    if ((blob.CopyState.Status == CopyStatus.Failed) || (blob.CopyState.Status == CopyStatus.Aborted))
                    {
                        throw new UserException("Copy of " + newContentName + " failed: " + blob.CopyState.StatusDescription);
                    }

                    if (blob.CopyState.Status == CopyStatus.Success)
                    {
                        break;
                    }
                }
            }

            await oldBlockBlobReference.DeleteAsync();
        }

        public async Task<int> MaintainAsync(CancellationToken cancellationToken)
        {
            var persistedCommittedContentNames =
                    _committedContentNamesRepository
                    .GetChronologicallyOrderedContentNames(_delegateContainer.Name, null, null, cancellationToken);

            var notPersistedContentNames =
                    GetAllContentNames(null)
                    .Except(persistedCommittedContentNames.OrderBy(x => x));

            var notPersistedCommittedContents=
                    GetContentIdentifiers(notPersistedContentNames)
                    .Where(x => !x.Uncommitted)
                    .OrderBy(x => x.ModifiedMoment)
                    .ThenBy(x => x.Guid)
                    .Select(x => new KeyValuePair<string, DateTimeOffset>(_contentNameProvider.GetContentName(x), x.ModifiedMoment));

            var result = _committedContentNamesRepository.AddContents(_delegateContainer.Name, notPersistedCommittedContents, cancellationToken);

            return await Task.FromResult(result);
        }

        public (byte[] Hash, int Count) GetMonthsHash(DateTimeOffset beforeMoment)
        {
            var beforeMomentMonth = _contentMonthProvider.GetContentMonth(beforeMoment);

            var persistedMonthHashAndCounts =
                    _contentsMonthHashRepository.GetMonthHashAndCounts(_delegateContainer.Name)
                    .OrderBy(x => x.Month)
                    .ToList();

            var persistedPastMonthHashAndCounts =
                    persistedMonthHashAndCounts
                    .Where(x => x.Month < beforeMomentMonth)
                    .ToList();

            var lastPersistedPastMonth = persistedPastMonthHashAndCounts.Select(x => (DateTimeOffset?)x.Month).LastOrDefault();

            var nextContentIdentifiers = GetChronologicallyOrderedContentIdentifiers(beforeMoment, lastPersistedPastMonth?.AddMonths(1).AddTicks(-1));
            var nextMonthHashAndCounts = _contentsMonthHashProvider.GetMonthHashAndCounts(nextContentIdentifiers).ToList();

            AddMissingMonthHashAndCounts(nextMonthHashAndCounts);

            var allMonthHashAndCounts = persistedPastMonthHashAndCounts.Concat(nextMonthHashAndCounts);

            return _hashProvider.GetHashAndCount(allMonthHashAndCounts);
        }

        private void AddMissingMonthHashAndCounts(IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            var missingMonthHashAndCounts =
                    monthHashAndCounts
                    .Where(x =>
                        monthHashAndCounts
                        .Any(y => (y.Month > x.Month) && (y.LastModifiedMoment > x.Month.AddMonths(1).AddDays(15)))
                    );
            _contentsMonthHashRepository.AddMonthHashAndCounts(_delegateContainer.Name, missingMonthHashAndCounts);
        }
    }
}
