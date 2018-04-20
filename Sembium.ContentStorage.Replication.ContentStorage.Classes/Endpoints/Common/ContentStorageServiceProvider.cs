using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.ContentStorage.Endpoints.Common
{
    public class ContentStorageServiceProvider : IContentStorageServiceProvider
    {
        private const string ContentStorageLocatorSettingName = "ContentStorageLocators";

        private readonly IConfigurationSettings _configurationSettings;

        public ContentStorageServiceProvider(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;
        }

        public string GetURL(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                return null;

            var containerNameParts = containerName.Split('@');

            if (containerNameParts.Count() == 2)
                return containerNameParts[1];

            var locatorURLs = _configurationSettings.GetAppSetting(ContentStorageLocatorSettingName);

            if (string.IsNullOrEmpty(locatorURLs))
            {
                throw new UserException(string.Format("App setting not found: {0}", ContentStorageLocatorSettingName));
            }

            locatorURLs = locatorURLs.Replace("{ContainerName}", containerName);

            var httpClient = new System.Net.Http.HttpClient();

            var errorMsg = string.Empty;

            var RetryCount = 2;
            for (var i = 0; i < RetryCount; i++)
            {
                foreach (var url in locatorURLs.Split(';'))
                {
                    try
                    {
                        return httpClient.GetStringAsync(url).Result;
                    }
                    catch (AggregateException e)
                    {
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            errorMsg = string.Join("\n", e.Flatten().InnerExceptions.Select(x => x.Message));
                        }
                    }
                    catch (Exception e)
                    {
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            errorMsg = e.Message;
                        }
                    }
                }

                Task.Delay(3000).Wait();
            }

            throw new UserException(string.Format("Could not obtain content storage service for container \"{0}\":\r\n{1}", containerName, errorMsg));
        }

        public string GetContainerName(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                return null;

            return containerName.Split('@')[0];
        }
    }
}
