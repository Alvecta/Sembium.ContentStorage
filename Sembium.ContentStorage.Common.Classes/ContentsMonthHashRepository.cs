using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sembium.ContentStorage.Misc;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Utils;

namespace Sembium.ContentStorage.Common
{
    public class ContentsMonthHashRepository : IContentsMonthHashRepository
    {
        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IHashStringProvider _hashStringProvider;
        private readonly IMonthHashAndCountFactory _monthHashAndCountFactory;

        public ContentsMonthHashRepository(
            ISystemContainerProvider systemContainerProvider,
            IHashStringProvider hashStringProvider,
            IMonthHashAndCountFactory monthHashAndCountFactory)
        {
            _systemContainerProvider = systemContainerProvider;
            _hashStringProvider = hashStringProvider;
            _monthHashAndCountFactory = monthHashAndCountFactory;
        }

        public void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            var lines = monthHashAndCounts.Select(x => GetMonthHashAndCountLine(x));

            var text = string.Join(null, lines);

            if (!string.IsNullOrEmpty(text))
            {
                using (var stream = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;

                    AddMonthHashAndCounts(containerName, stream);
                }
            }
        }

        private void AddMonthHashAndCounts(string containerName, Stream stream)
        {
            var hashContainer = GetCacheContainer(containerName);
            var content = hashContainer.GetContent(GenerateGuid() + ".txt");
            content.LoadFromStream(stream);
        }

        public IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName)
        {
            var hashContainer = GetCacheContainer(containerName);
            var contents = hashContainer.GetContents(null);
            return GetMonthHashAndCounts(contents);
        }

        private IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(IEnumerable<IContent> contents)
        {
            return
                contents
                .SelectMany(x => GetAllMonthHashAndCounts(x))
                .GroupBy(x => x.Month)
                .Select(x => x.First());
        }

        private ISystemContainer GetCacheContainer(string containerName)
        {
            return _systemContainerProvider.GetSystemContainer("hashes/" + containerName);
        }

        private IEnumerable<IMonthHashAndCount> GetAllMonthHashAndCounts(IContent content)
        {
            using (var stream = content.GetReadStream(true))
            {
                foreach (var line in stream.ReadAllLines(Encoding.UTF8))  // to prevent stream disposal
                {
                    yield return GetMonthHashAndCount(line);
                }
            }
        }

        private IMonthHashAndCount GetMonthHashAndCount(string line)
        {
            var parts = line.Split('=');

            var month = DateTimeOffset.ParseExact(parts[0], "yyyy-MM", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

            var hashAndCount = _hashStringProvider.GetStringHashAndCount(parts[1]);

            return _monthHashAndCountFactory(month, hashAndCount.Hash, hashAndCount.Count, null);
        }

        private string GetMonthHashAndCountLine(IMonthHashAndCount monthHashAndCount)
        {
            return
                monthHashAndCount.Month.ToUniversalTime().ToString("yyyy-MM") +
                "=" +
                _hashStringProvider.GetHashString(monthHashAndCount.Hash, monthHashAndCount.Count) +
                Environment.NewLine;
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public async Task CompactAsync(string containerName, CancellationToken cancellationToken)
        {
            var hashContainer = GetCacheContainer(containerName);
            var contents = hashContainer.GetContents(null).Select(x => x as ISystemContent).ToList();  // enumerate once

            var monthHashAndCounts = GetMonthHashAndCounts(contents);

            AddMonthHashAndCounts(containerName, monthHashAndCounts);

            await DeleteContents(contents, cancellationToken);
        }

        private async Task DeleteContents(IEnumerable<ISystemContent> contents, CancellationToken cancellationToken)
        {
            var tasks = contents.AsParallel().Select(x => x.DeleteAsync(cancellationToken));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch
            {
                // do nothing, delete tomorrow
            }
        }
    }
}
