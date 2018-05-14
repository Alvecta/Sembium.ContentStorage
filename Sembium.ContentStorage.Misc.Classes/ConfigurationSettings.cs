using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAppSetting(string settingName, string defaultValue = null)
        {
            var result = _configuration.GetSection("AppSettings")?.GetValue<string>(settingName);

            if (string.IsNullOrEmpty(result))
            {
                if (defaultValue != null)
                {
                    return defaultValue;
                }

                throw new Exception($"AppSetting '{settingName}' not found or has no value");
            }

            return result;
        }

        public string GetConnectionString(string connectionStringName)
        {
            return _configuration.GetConnectionString(connectionStringName);
        }
    }
}
