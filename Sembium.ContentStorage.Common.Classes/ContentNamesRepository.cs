using Microsoft.Extensions.Options;
using Sembium.ContentStorage.Storage.Tools;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class ContentNamesRepository : IContentNamesRepository
    {
        private readonly ContentNamesRepositorySettings _contentNamesRepositorySettings;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IContentMonthProvider _contentMonthProvider;
        private readonly IContentNamesVault _contentNamesVault;

        public ContentNamesRepository(
            IOptions<ContentNamesRepositorySettings> contentNamesRepositorySettings,
            IContentNameProvider contentNameProvider,
            IContentMonthProvider contentMonthProvider,
            IContentNamesVault contentNamesVault)
        {
            _contentNamesRepositorySettings = contentNamesRepositorySettings.Value;
            _contentNameProvider = contentNameProvider;
            _contentMonthProvider = contentMonthProvider;
            _contentNamesVault = contentNamesVault;
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private string GenerateContentNamesVaultItemName(string prefix)
        {
            return prefix + GenerateGuid() + ".txt";
        }

        private IContentNamesVaultItem GetAppendContentNamesVaultItem(string contentsContainerName, DateTimeOffset contentMonth)
        {
            var prefix = contentMonth.ToUniversalTime().ToString("yyyy-MM-");

            var availableContentNamesVaultItems =
                    _contentNamesVault
                    .GetItems(contentsContainerName, prefix)
                    .Where(x => x.CanAppend())
                    .ToList();

            if (availableContentNamesVaultItems.Count() < _contentNamesRepositorySettings.ActiveMonthVaultItemCount)  // todo: config
            {
                return _contentNamesVault.GetNewItem(contentsContainerName, GenerateContentNamesVaultItemName(prefix));
            }
            else
            {
                return availableContentNamesVaultItems[new Random().Next(availableContentNamesVaultItems.Count())];
            }
        }

        private void AddBlock(string contentsContainerName, string blockText, DateTimeOffset contentMonth, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(blockText);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                var contentNamesVaultItem = GetAppendContentNamesVaultItem(contentsContainerName, contentMonth);
                contentNamesVaultItem.Append(stream);
            }
        }

        private int AddContents(string contentsContainerName, IEnumerable<string> contentNames, DateTimeOffset contentMonth, CancellationToken cancellationToken)
        {
            var result = 0;
            string text = "";
            foreach (var contentName in contentNames)
            {
                var newText = text + contentName + Environment.NewLine;

                if (Encoding.UTF8.GetByteCount(newText) > 4 * 1024 * 1024)
                {
                    AddBlock(contentsContainerName, text, contentMonth, cancellationToken);
                    text = contentName + Environment.NewLine;
                }
                else
                {
                    text = newText;
                }

                result++;
            }

            if (text != "")
            {
                AddBlock(contentsContainerName, text, contentMonth, cancellationToken);
            }

            return result;
        }

        public void AddContent(string contentsContainerName, string contentName, DateTimeOffset contentDate, CancellationToken cancellationToken)
        {
            AddContents(contentsContainerName, new[] { contentName }, _contentMonthProvider.GetContentMonth(contentDate), cancellationToken);
        }

        public int AddContents(string contentsContainerName, IEnumerable<KeyValuePair<string, DateTimeOffset>> contents, CancellationToken cancellationToken)
        {
            var result = 0;

            var monthContents = 
                    contents
                    .OrderBy(x => x.Value)
                    .Select(x => new { ContentName = x.Key, ContentMonth = _contentMonthProvider.GetContentMonth(x.Value) })
                    .GroupBy(x => x.ContentMonth);

            foreach (var month in monthContents)
            {
                result += AddContents(contentsContainerName, month.Select(y => y.ContentName), month.Key, cancellationToken);
            }

            return result;
        }

        private IOrderedEnumerable<(DateTimeOffset Month, IEnumerable<IContentNamesVaultItem> VaultItems)> GetMonthVaultItems(string contentsContainerName, DateTimeOffset? beforeMonth, DateTimeOffset? afterMonth)
        {
            var contentNamesVaultItems =
                    _contentNamesVault.GetItems(contentsContainerName, "")
                    .Select(x => new { ContentNamesVaultItem = x, ContentsMonth = GetContentsMonth(x.Name) });

            if (beforeMonth.HasValue)
            {
                contentNamesVaultItems = contentNamesVaultItems.Where(x => x.ContentsMonth < beforeMonth.Value);
            }

            if (afterMonth.HasValue)
            {
                contentNamesVaultItems = contentNamesVaultItems.Where(x => x.ContentsMonth > afterMonth.Value);
            }

            return
                contentNamesVaultItems
                .GroupBy(x => x.ContentsMonth)
                .Select(x => (Month: x.Key, VaultItems: x.Select(y => y.ContentNamesVaultItem)))
                .OrderBy(x => x.Month);
        }

        public IEnumerable<string> GetChronologicallyOrderedContentNames(string contentsContainerName, DateTimeOffset? beforeMonth, DateTimeOffset? afterMonth, CancellationToken cancellationToken)
        {
            var monthContentNamesVaultItems = GetMonthVaultItems(contentsContainerName, beforeMonth, afterMonth);

            var result =
                    monthContentNamesVaultItems
                    .SelectMany(x =>
                        x.VaultItems
                        .AsParallel()
                        .SelectMany(y => GetContentNames(y, cancellationToken))
                        .Select(y => new { ContentName = y, ContentIdentifier = _contentNameProvider.GetContentIdentifier(y) })
                        .OrderBy(y => y.ContentIdentifier.ModifiedMoment)
                        .ThenBy(y => y.ContentIdentifier.Guid)
                        .Select(y => y.ContentName)
                        .UniqueOnOrdered()
                    );

            return result;
        }

        private IEnumerable<string> GetContentNamesVaultItemLines(IContentNamesVaultItem contentNamesVaultItem, CancellationToken cancellationToken)
        {
            using (var stream = contentNamesVaultItem.OpenReadStream())
            {
                foreach (var line in stream.ReadAllLines(Encoding.UTF8))  // to prevent stream disposal
                {
                    yield return line;
                }
            }
        }

        private IEnumerable<string> GetContentNames(IContentNamesVaultItem contentNamesVaultItem, CancellationToken cancellationToken)
        {
            return 
                GetContentNamesVaultItemLines(contentNamesVaultItem, cancellationToken)
                .Where(x => !string.IsNullOrEmpty(x));
        }

        private DateTimeOffset GetContentsMonth(string contentNamesVaultItemName)
        {
            var parts = contentNamesVaultItemName.Split('/').Last().Split('-');

            if (parts.Length < 2)
            {
                throw new UserException($"Invalid content names vault item name: {contentNamesVaultItemName}");
            }

            var year = int.Parse(parts[0]);
            var month = int.Parse(parts[1]);

            return new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.FromHours(0));
        }
    }
}
