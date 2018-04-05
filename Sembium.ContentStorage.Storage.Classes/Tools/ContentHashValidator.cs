using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public class ContentHashValidator : IContentHashValidator
    {
        private readonly IHashProvider _hashProvider;
        private readonly IHashStringProvider _hashStringProvider;

        public ContentHashValidator(
            IHashProvider hashProvider,
            IHashStringProvider hashStringProvider)
        {
            _hashProvider = hashProvider;
            _hashStringProvider = hashStringProvider;
        }

        private byte[] GetContentHash(IContent content)
        {
            using (var contentStream = content.GetReadStream())
            {
                return _hashProvider.GetHash(contentStream);
            }
        }

        public void ValidateHash(IContent content, string hash)
        {
            var contentHash = GetContentHash(content);
            var contentHashString = _hashStringProvider.GetHashString(contentHash);

            if (!string.Equals(contentHashString, hash, StringComparison.InvariantCultureIgnoreCase))
                throw new UserException("Invalid content hash");
        }
    }
}
