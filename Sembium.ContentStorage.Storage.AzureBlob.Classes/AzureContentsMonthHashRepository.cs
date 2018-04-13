using Microsoft.WindowsAzure.Storage.Blob;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.HostingResults.Factories;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContentsMonthHashRepository : IContentsMonthHashRepository
    {
        private const string ConnectionStringName = "AzureBlobStorage";
        private const string HashesSystemContainerName = "system-hashes";

        private readonly IConfigurationSettings _configurationSettings;
        private readonly IHashStringProvider _hashStringProvider;
        private readonly IMonthHashAndCountFactory _monthHashAndCountFactory;

        public AzureContentsMonthHashRepository(
            IConfigurationSettings configurationSettings,
            IHashStringProvider hashStringProvider,
            IMonthHashAndCountFactory monthHashAndCountFactory)
        {
            _configurationSettings = configurationSettings;
            _hashStringProvider = hashStringProvider;
            _monthHashAndCountFactory = monthHashAndCountFactory;
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            var connectionString = _configurationSettings.GetConnectionString(ConnectionStringName);

            var account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            return account.CreateCloudBlobClient();
        }

        private CloudBlobContainer GetHashesContainer()
        {
            return GetCloudBlobClient().GetContainerReference(HashesSystemContainerName);
        }

        private CloudAppendBlob GetHashesAppendBlobReference(string containerName)
        {
            var hashesContainer = GetHashesContainer();

            hashesContainer.CreateIfNotExistsAsync().Wait();

            var blobName = containerName + "-hashes.txt";
            return hashesContainer.GetAppendBlobReference(blobName);
        }

        public void AddMonthHashAndCounts(string containerName, IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            var lines = monthHashAndCounts.Select(x => GetMonthHashAndCountLine(x));

            var text = string.Join(null, lines);

            if (!string.IsNullOrEmpty(text))
            {
                var blob = GetHashesAppendBlobReference(containerName);

                if (!blob.ExistsAsync().Result)
                {
                    blob.CreateOrReplaceAsync().Wait();
                }

                blob.AppendTextAsync(text).Wait();
            }
        }

        private string GetMonthHashAndCountLine(IMonthHashAndCount monthHashAndCount)
        {
            return
                monthHashAndCount.Month.ToUniversalTime().ToString("yyyy-MM") +
                "=" +
                _hashStringProvider.GetHashString(monthHashAndCount.Hash, monthHashAndCount.Count) +
                Environment.NewLine;
        }

        public IEnumerable<IMonthHashAndCount> GetMonthHashAndCounts(string containerName)
        {
            return
                GetAllMonthHashAndCounts(containerName)
                .GroupBy(x => x.Month)
                .Select(x => x.Last());
        }

        private IEnumerable<IMonthHashAndCount> GetAllMonthHashAndCounts(string containerName)
        {
            var blob = GetHashesAppendBlobReference(containerName);

            if (blob.ExistsAsync().Result)
            {
                using (var stream = new MemoryStream())
                {
                    blob.DownloadToStreamAsync(stream).Wait();
                    foreach (var line in stream.ReadAllLines(Encoding.UTF8))  // to prevent stream disposal
                    {
                        yield return GetMonthHashAndCount(line);
                    }
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
    }
}
