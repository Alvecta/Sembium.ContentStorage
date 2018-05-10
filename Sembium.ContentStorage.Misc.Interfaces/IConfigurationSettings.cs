using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public interface IConfigurationSettings
    {
        string GetAppSetting(string settingName);
        string GetConnectionString(string connectionStringName);
    }
}
