using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Replicator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    public class FileStoredCompleteMomentConfigProvider : IFileStoredCompleteMomentConfigProvider
    {
        private const string SCompleteMomentExpiryMinutesAppSetting = "CompleteMomentExpiryMinutes";

        private readonly IFileStoredCompleteMomentConfigFactory _fileStoredCompleteMomentConfigFactory;
        private readonly IConfigurationSettings _configurationSettings;

        public FileStoredCompleteMomentConfigProvider(
            IFileStoredCompleteMomentConfigFactory fileStoredCompleteMomentConfigFactory,
            IConfigurationSettings configurationSettings)
        {
            _fileStoredCompleteMomentConfigFactory = fileStoredCompleteMomentConfigFactory;
            _configurationSettings = configurationSettings;
        }

        public IFileStoredCompleteMomentConfig GetConfig()
        {
            return _fileStoredCompleteMomentConfigFactory(GetFileName(), GetExpiryMinutes());
        }

        private string GetFileName()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return System.IO.Path.Combine(appDataFolder, Utils.GetAssemblyCompany(), Utils.GetAssemblyTitle(), "CompleteMoments.dat");
        }

        private int GetExpiryMinutes()
        {
            return int.Parse(_configurationSettings.GetAppSetting(SCompleteMomentExpiryMinutesAppSetting));
        }
    }
}
