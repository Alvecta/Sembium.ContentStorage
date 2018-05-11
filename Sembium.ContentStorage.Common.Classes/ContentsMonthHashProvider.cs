using Sembium.ContentStorage.Misc;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class ContentsMonthHashProvider : IContentsMonthHashProvider
    {
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IContentMonthProvider _contentMonthProvider;
        private readonly IHashProvider _hashProvider;
        private readonly IMonthHashAndCountFactory _monthHashAndCountFactory;

        public ContentsMonthHashProvider(
            IContentIdentifierGenerator contentIdentifierGenerator,
            IContentMonthProvider contentMonthProvider,
            IHashProvider hashProvider,
            IMonthHashAndCountFactory monthHashAndCountFactory)
        {
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _contentMonthProvider = contentMonthProvider;
            _hashProvider = hashProvider;
            _monthHashAndCountFactory = monthHashAndCountFactory;
        }

        public IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(IEnumerable<IContentIdentifier> contentIdentifiers)
        {
            return
                contentIdentifiers
                .Where(x => !_contentIdentifierGenerator.IsSystemContent(x))
                .GroupBy(x => _contentMonthProvider.GetContentMonth(x.ModifiedMoment))
                .Select(x =>
                    new {
                        Month = x.Key,
                        HashAndCount =
                            _hashProvider.GetHashAndCount(
                                x.OrderBy(y => y.ModifiedMoment)
                                .ThenBy(y => y.Guid)
                                .Select(y => Encoding.Unicode.GetBytes(y.Hash))
                            ),
                        LastModifiedMoment = x.Max(y => y.ModifiedMoment)
                    })
                .OrderBy(x => x.Month)
                .Select(x => _monthHashAndCountFactory(x.Month, x.HashAndCount.Hash, x.HashAndCount.Count, x.LastModifiedMoment));
        }
    }
}
