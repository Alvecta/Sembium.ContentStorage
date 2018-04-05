using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public class AmazonContainer : IContainer
    {
        private readonly string _bucketName;
        private readonly string _directoryName;
        private readonly Amazon.S3.IAmazonS3 _amazonS3;
        private readonly IAmazonContentFactory _amazonContentFactory;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IContentHashValidator _contentHashValidator;
        private readonly IContentsMonthHashProvider _contentsMonthHashProvider;
        private readonly IHashProvider _hashProvider;

        public AmazonContainer(string bucketName, string directoryName,
            Amazon.S3.IAmazonS3 amazonS3,
            IAmazonContentFactory amazonContentFactory,
            IContentNameProvider contentNameProvider,
            IContentIdentifierGenerator contentIdentifierGenerator,
            IContentHashValidator contentHashValidator,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IHashProvider hashProvider)
        {
            _bucketName = bucketName;
            _directoryName = directoryName;
            _amazonS3 = amazonS3;
            _amazonContentFactory = amazonContentFactory;
            _contentNameProvider = contentNameProvider;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _contentHashValidator = contentHashValidator;
            _contentsMonthHashProvider = contentsMonthHashProvider;
            _hashProvider = hashProvider;
        }

        private bool ContentExists(string contentName)
        {
            if (contentName == null)
                return false;

            return _amazonS3.GetObjectExists(_bucketName, MakeKey(contentName));
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

            return _amazonContentFactory(_bucketName, MakeKey(contentName));
        }

        public IContent GetContent(IContentIdentifier contentIdentifier)
        {
            if (!ContentExists(contentIdentifier))
                throw new UserException("Content does not exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            return _amazonContentFactory(_bucketName, MakeKey(contentName));
        }

        public IEnumerable<IContentIdentifier> GetContentIdentifiers(bool committed, string hash)
        {
            return GetContentIdentifiers(committed, hash, null, null);
        }

        public IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment)
        {
            return
                GetContentIdentifiers(true, null, beforeMoment, afterMoment)
                .OrderBy(x => x.ModifiedMoment)
                .ThenBy(x => x.Guid);
        }

        private IEnumerable<IContentIdentifier> GetContentIdentifiers(bool committed, string hash, DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment)
        {
            var prefix = _contentNameProvider.GetSearchPrefix(hash);

            var request = new Amazon.S3.Model.ListObjectsRequest { BucketName = _bucketName, Prefix = MakeKey(prefix) };

            while (true)
            {
                var response = _amazonS3.ListObjectsAsync(request).Result;

                var contentIdentifiers =
                    response.S3Objects.Select(x => _contentNameProvider.GetContentIdentifier(x.Key.Split('/').Last()))
                    .Where(x => x != null)
                    .Where(x => x.Uncommitted != committed)
                    .Where(x => (!beforeMoment.HasValue) || (x.ModifiedMoment < beforeMoment.Value))
                    .Where(x => (!afterMoment.HasValue) || (x.ModifiedMoment > afterMoment.Value));

                foreach (var identifier in contentIdentifiers)
                {
                    yield return identifier;
                }

                if (!response.IsTruncated)
                    break;

                request.Marker = response.NextMarker;
            }
        }

        public async Task<IContentIdentifier> CommitContentAsync(IContentIdentifier uncommittedContentIdentifier)
        {
            System.Diagnostics.Debug.Assert(uncommittedContentIdentifier.Uncommitted);

            var unncommittedContent = GetContent(uncommittedContentIdentifier);
            _contentHashValidator.ValidateHash(unncommittedContent, uncommittedContentIdentifier.Hash);

            var uncommittedContentName = _contentNameProvider.GetContentName(uncommittedContentIdentifier);
            var contentIdentifier = _contentIdentifierGenerator.GetCommittedContentIdentifier(uncommittedContentIdentifier);
            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            RenameContent(uncommittedContentName, contentName);

            return await Task.FromResult(contentIdentifier);
        }

        private void RenameContent(string oldContentName, string newContentName)
        {
            var copyObjectRequest = 
                new Amazon.S3.Model.CopyObjectRequest()
                {
                    SourceBucket = _bucketName,
                    SourceKey = MakeKey(oldContentName),
                    DestinationBucket = _bucketName,
                    DestinationKey = MakeKey(newContentName)
                };

            _amazonS3.CopyObjectAsync(copyObjectRequest).Wait();

            var deleteObjectRequest = 
                new Amazon.S3.Model.DeleteObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = MakeKey(oldContentName)
                };

            _amazonS3.DeleteObjectAsync(deleteObjectRequest).Wait();
        }

        public Task<int> MaintainAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public (byte[] Hash, int Count) GetMonthsHash(DateTimeOffset beforeMoment)
        {
            var monthHashAndCounts =
                    _contentsMonthHashProvider.GetMonthHashAndCounts(
                        GetChronologicallyOrderedContentIdentifiers(beforeMoment, null)
                    );

            return _hashProvider.GetHashAndCount(monthHashAndCounts);
        }

        private string MakeKey(string contentName)
        {
            return _directoryName + "/" + contentName;
        }
    }
}
