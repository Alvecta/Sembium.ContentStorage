﻿using Microsoft.WindowsAzure.Storage.Blob;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using System.Collections.Generic;
using System.Linq;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContentStorageHost : IContentStorageHost
    {
        private const string ConnectionStringName = "AzureBlobStorage";

        private readonly IConfigurationSettings _configurationSettings;
        private readonly IAzureContainerFactory _azureContainerFactory;

        public AzureContentStorageHost(
            IConfigurationSettings configurationSettings, 
            IAzureContainerFactory azureContainerFactory)
        {
            _configurationSettings = configurationSettings;
            _azureContainerFactory = azureContainerFactory;
        }

        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient GetCloudBlobClient()
        {
            var connectionString = _configurationSettings.GetConnectionString(ConnectionStringName);

            var account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            return account.CreateCloudBlobClient();            
        }

        private string FixContainerName(string containerName)
        {
            return containerName.ToLowerInvariant();  // development storage throws "Bad request. 400." on camelcase names
        }

        public bool ContainerExists(string containerName)
        {
            var cloudBlobContainer = GetCloudBlobClient().GetContainerReference(FixContainerName(containerName));

            return cloudBlobContainer.ExistsAsync().Result;
        }

        public IContainer CreateContainer(string containerName)
        {
            var cloudBlobContainer = GetCloudBlobClient().GetContainerReference(FixContainerName(containerName));

            if (cloudBlobContainer.ExistsAsync().Result)
                throw new UserException("Container already exist");

            cloudBlobContainer.CreateAsync().Wait();

            return _azureContainerFactory(cloudBlobContainer);
        }

        public IContainer GetContainer(string containerName)
        {
            var cloudBlobContainer = GetCloudBlobClient().GetContainerReference(FixContainerName(containerName));

            if (!cloudBlobContainer.ExistsAsync().Result)
                throw new UserException("Container does not exist: " + containerName);

            return _azureContainerFactory(cloudBlobContainer);
        }

        public IEnumerable<string> GetContainerNames()
        {
            return GetAllBlobContainers().Select(x => x.Name).OrderBy(x => x);
        }

        private IEnumerable<CloudBlobContainer> GetAllBlobContainers()
        {
            var cloudBlobClient = GetCloudBlobClient();

            BlobContinuationToken continuationToken = null;

            while (true)
            {
                var result = cloudBlobClient.ListContainersSegmentedAsync(continuationToken).Result;

                foreach (var item in result.Results)
                {
                    yield return item;
                }

                if (result.ContinuationToken == null)
                    break;

                continuationToken = result.ContinuationToken;
            }
        }

        public IHttpRequestInfo GetUrlContentUploadInfo(string contentUrl, string contentStorageServiceUrl, string containerName, string contentID, long size, string authenticationToken)
        {
            return null;
        }
    }
}