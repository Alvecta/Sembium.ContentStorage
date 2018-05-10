using Microsoft.WindowsAzure.Storage.Blob;
using Sembium.ContentStorage.Misc;
using Sembium.ContentStorage.Storage.ContentNames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContentNamesVault : IContentNamesVault
    {
        private const string ConnectionStringName = "AzureBlobStorage";
        private const string NamesSystemContainerName = "system-names";
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IAzureContentNamesVaultItemFactory _azureContentNamesVaultItemFactory;

        public AzureContentNamesVault(
            IConfigurationSettings configurationSettings,
            IAzureContentNamesVaultItemFactory azureContentNamesVaultItemFactory)
        {
            _configurationSettings = configurationSettings;
            _azureContentNamesVaultItemFactory = azureContentNamesVaultItemFactory;
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            var connectionString = _configurationSettings.GetConnectionString(ConnectionStringName);

            var account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            return account.CreateCloudBlobClient();
        }

        private CloudBlobContainer GetNamesContainer()
        {
            return GetCloudBlobClient().GetContainerReference(NamesSystemContainerName);
        }

        public IEnumerable<IContentNamesVaultItem> GetItems(string contentsContainerName, string prefix)
        {
            var namesContainer = GetNamesContainer();

            if (!namesContainer.ExistsAsync().Result)
            {
                return Enumerable.Empty<IContentNamesVaultItem>();
            }

            return
                GetAllAppendBlobs(namesContainer, contentsContainerName, prefix)
                .Select(x => _azureContentNamesVaultItemFactory(x));
        }

        private IEnumerable<CloudAppendBlob> GetAllAppendBlobs(CloudBlobContainer cloudBlobContainer, string contentsContainerName, string prefix)
        {
            BlobContinuationToken continuationToken = null;

            while (true)
            {
                var result = cloudBlobContainer.ListBlobsSegmentedAsync(contentsContainerName + '/' + prefix, false, BlobListingDetails.None, null, continuationToken, null, null).Result;

                var blobs = result.Results.OfType<CloudAppendBlob>();

                foreach (var blob in blobs)
                {
                    yield return blob;
                }

                if (result.ContinuationToken == null)
                    break;

                continuationToken = result.ContinuationToken;
            }
        }

        public IContentNamesVaultItem GetNewItem(string contentsContainerName, string name)
        {
            var namesContainer = GetNamesContainer();
            namesContainer.CreateIfNotExistsAsync().Wait();

            var blob = namesContainer.GetAppendBlobReference(contentsContainerName + '/' + name);

            return _azureContentNamesVaultItemFactory(blob);
        }
    }
}
