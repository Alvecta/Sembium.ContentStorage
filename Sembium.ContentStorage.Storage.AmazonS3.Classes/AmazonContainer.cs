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
    public class AmazonContainer : IContainer, ISystemContainer
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

            return _amazonContentFactory(_bucketName, MakeKey(contentName), null);
        }

        public IContent GetContent(IContentIdentifier contentIdentifier)
        {
            if (!ContentExists(contentIdentifier))
                throw new UserException("Content does not exists");

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);

            return GetContent(contentName, null);
        }

        public IContent GetContent(string contentName)
        {
            return _amazonContentFactory(_bucketName, MakeKey(contentName), null);
        }

        private IContent GetContent(string contentName, long? size)
        {
            return _amazonContentFactory(_bucketName, MakeKey(contentName), size);
        }

        public IEnumerable<string> GetContentNames(string prefix)
        {
            return InternalGetContents(prefix).Select(x => x.ContentName);
        }

        private IEnumerable<(string ContentName, long? Size)> InternalGetContents(string prefix)
        { 
            var request = new Amazon.S3.Model.ListObjectsV2Request { BucketName = _bucketName, Prefix = MakeKey(prefix) };

            var containerDepth = _directoryName.Split('/').Length - 1;

            while (true)
            {
                var response = _amazonS3.ListObjectsV2Async(request).Result;

                var contents = 
                        response.S3Objects
                        .Select(x => (ContentName: string.Join("/", x.Key.Split('/').Skip(1 + containerDepth)), Size: x.Size))
                        .Where(x => !string.IsNullOrEmpty(x.ContentName));

                foreach (var content in contents)
                {
                    yield return content;
                }

                if (!response.IsTruncated)
                    break;

                request.ContinuationToken = response.ContinuationToken;
            }
        }

        public IEnumerable<IContent> GetContents(string prefix)
        {
            return InternalGetContents(prefix).Select(x => GetContent(x.ContentName, x.Size));
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

        private string MakeKey(string contentName)
        {
            return _directoryName + "/" + contentName;
        }
    }
}
