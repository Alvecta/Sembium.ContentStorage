using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Main
{
    public class LogFileNameProvider : ILogFileNameProvider
    {
        private readonly IConfigResolver _configResolver;

        public LogFileNameProvider(IConfigResolver configResolver)
        {
            _configResolver = configResolver;
        }

        public string GetLogFileName()
        {
            var config = _configResolver.GetConfig();

            if ((config.RouteConfigs != null) && (config.RouteConfigs.Where(x => !x.HashCheckMoment.HasValue).Any()))
                return config.LogFileName;

            var extension = System.IO.Path.GetExtension(config.LogFileName);
            var checkExtension = ".check" + extension;
            return System.IO.Path.ChangeExtension(config.LogFileName, checkExtension);
        }
    }
}
