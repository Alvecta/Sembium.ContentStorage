using Sembium.ContentStorage.Misc;
using Sembium.ContentStorage.Misc.Utils;
using Sembium.ContentStorage.Storage.FileSystem.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class TransferServiceProvider : ITransferServiceProvider
    {
        private const string TransferServiceURLSettingName = "TransferServiceURL";

        private readonly ISignedURLProvider _signedURLProvider;
        private readonly IConfigurationSettings _configurationSettings;

        public TransferServiceProvider(
            ISignedURLProvider signedURLProvider,
            IConfigurationSettings configurationSettings)
        {
            _signedURLProvider = signedURLProvider;
            _configurationSettings = configurationSettings;
        }

        public string GetURL(string containerName, string contentName, string operation, int expirySeconds)
        {
            var fileUrl = string.Format("{0}/{1}/{2}/{3}", GetTransferServiceBaseURL(), operation, containerName, contentName);

            var now = DateTimeOffset.Now;

            return _signedURLProvider.GetSignedURL(fileUrl, now.AddMinutes(-1), now.AddSeconds(expirySeconds));
        }

        private string GetTransferServiceBaseURL()  
        {
            var transferServiceURL = _configurationSettings.GetAppSetting(TransferServiceURLSettingName);

            if (string.IsNullOrEmpty(transferServiceURL))
                throw new UserException("Transfer service address not configured");

            return transferServiceURL.TrimEnd('/');
        }
    }
}
