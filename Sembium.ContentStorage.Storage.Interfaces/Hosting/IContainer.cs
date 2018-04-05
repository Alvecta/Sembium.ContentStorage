using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IContainer
    {
        bool ContentExists(IContentIdentifier contentIdentifier);
        IContent CreateContent(IContentIdentifier contentIdentifier);
        Task<IContentIdentifier> CommitContentAsync(IContentIdentifier uncommittedContentIdentifier);
        IContent GetContent(IContentIdentifier contentIdentifier);
        IEnumerable<IContentIdentifier> GetContentIdentifiers(bool committed, string hash);
        IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment);
        (byte[] Hash, int Count) GetMonthsHash(DateTimeOffset beforeMoment);
        Task<int> MaintainAsync(CancellationToken cancellationToken);
    }
}
