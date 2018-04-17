using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;

namespace Sembium.ContentStorage.Storage.Common
{
    public class ContentIdentifiersProvider : IContentIdentifiersProvider
    {
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;

        public ContentIdentifiersProvider(
            IContentNameProvider contentNameProvider,
            IContentIdentifierGenerator contentIdentifierGenerator)
        {
            _contentNameProvider = contentNameProvider;
            _contentIdentifierGenerator = contentIdentifierGenerator;
        }

        public IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(IContainer container, DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment)
        {
            var result =
                    GetContentIdentifiers(container, null)
                    .Where(x => !x.Uncommitted)
                    .Where(x => !_contentIdentifierGenerator.IsSystemContent(x));

            if (beforeMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment < beforeMoment.Value);
            }

            if (afterMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment > afterMoment.Value);
            }

            return 
                result
                .OrderBy(x => x.ModifiedMoment)
                .ThenBy(x => x.Guid);
        }

        public IEnumerable<IContentIdentifier> GetContentIdentifiers(IContainer container, bool committed, string hash)
        {
            var prefix = _contentNameProvider.GetSearchPrefix(hash);

            prefix = prefix?.ToLower();

            return
                GetContentIdentifiers(container, prefix)
                .Where(x => x.Uncommitted != committed);
        }

        public IEnumerable<IContentIdentifier> GetContentIdentifiers(IEnumerable<string> contentNames)
        {
            return
                contentNames
                .Select(x => _contentNameProvider.GetContentIdentifier(x))
                .Where(x => x != null);
        }

        private IEnumerable<IContentIdentifier> GetContentIdentifiers(IContainer container, string prefix)
        {
            return GetContentIdentifiers(container.GetContentNames(prefix));
        }
    }
}
