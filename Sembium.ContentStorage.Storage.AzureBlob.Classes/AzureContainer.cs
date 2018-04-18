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
    public class AzureContainer : IContainer, IMultiPartUploadContainer, ISystemContainer
    {
        private const string AzureApiVersion = "2017-07-29";

        private readonly Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer _delegateContainer;
        private readonly string _rootPath;
        private readonly IAzureContentFactory _azureContentFactory;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IContentHashValidator _contentHashValidator;
        private readonly IHashProvider _hashProvider;

        public AzureContainer(
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer delegateContainer,
            string rootPath,
            IAzureContentFactory azureContentFactory,
            IContentNameProvider contentNameProvider,
            IContentIdentifierGenerator contentIdentifierGenerator,
            IContentHashValidator contentHashValidator,
            IHashProvider hashProvider)
        {
            _delegateContainer = delegateContainer;
            _rootPath = rootPath;
            _azureContentFactory = azureContentFactory;
            _contentNameProvider = contentNameProvider;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _contentHashValidator = contentHashValidator;
            _hashProvider = hashProvider;
        }

        private bool ContentExists(string contentName)
        {
            if (contentName == null)
                return false;

            var blockBlobReference = GetBlockBlobReference(contentName);
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
            var blockBlobReference = GetBlockBlobReference(contentName);

            return _azureContentFactory(blockBlobReference);
        }

        private CloudBlockBlob GetBlockBlobReference(string contentName)
        {
            return _delegateContainer.GetBlockBlobReference(contentName);
        }

        public IContent GetContent(IContentIdentifier contentIdentifier)
        {
            if (!ContentExists(contentIdentifier))
                throw new UserException("Content does not exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            return GetContent(contentName);
        }

        public IContent GetContent(string contentName)
        {
            var contentFullName = string.IsNullOrEmpty(_rootPath) ? contentName : _rootPath + "/" + contentName;
            var blockBlobReference = GetBlockBlobReference(contentFullName);
            return _azureContentFactory(blockBlobReference);
        }

        public IEnumerable<string> GetContentNames(string prefix)
        {
            return GetCloudBlobs(prefix).Select(x => x.Name);
        }

        public IEnumerable<IContent> GetContents(string prefix)
        {
            return GetCloudBlobs(prefix).Select(x => _azureContentFactory(x));
        }

        private IEnumerable<CloudBlob> GetCloudBlobs(string prefix)
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

            return contentIdentifier;
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
            var oldBlockBlobReference = GetBlockBlobReference(oldContentName);
            var newBlockBlobReference = GetBlockBlobReference(newContentName);

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
    }
}
