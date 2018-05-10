using Sembium.ContentStorage.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem
{
    public class FileSystemStorageRootProvider : IFileSystemStorageRootProvider
    {
        private const string StorageRootAppSettingName = "StorageRoot";

        private readonly IConfigurationSettings _configurationSettings;

        public FileSystemStorageRootProvider(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;
        }

        public string GetRoot()
        {
            return _configurationSettings.GetAppSetting(StorageRootAppSettingName);
        }
    }
}
