using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.Common
{
    public interface IContentIdentifiersProvider
    {
        IEnumerable<IContentIdentifier> GetContentIdentifiers(IEnumerable<string> contentNames);
        IEnumerable<IContentIdentifier> GetContentIdentifiers(IContainer container, bool committed, string hash);
        IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(IContainer container, DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment);
    }
}
