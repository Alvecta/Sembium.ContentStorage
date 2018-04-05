using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        private readonly IConfiguration _configuration;

        public ConfigurationSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAppSetting(string settingName)
        {
            return _configuration.GetSection("AppSettings").GetValue<string>(settingName);
        }

        public string GetConnectionString(string connectionStringName)
        {
            return _configuration.GetConnectionString(connectionStringName);
        }
    }
}
