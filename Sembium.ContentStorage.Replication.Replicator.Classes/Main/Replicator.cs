using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Common.Exceptions;
using Sembium.ContentStorage.Replication.Common.Main;
using Sembium.ContentStorage.Replication.Replicator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Main
{
    public class Replicator : IReplicator
    {
        private readonly IReplicationWorker _replicationWorker;
        private readonly IConfigResolver _configResolver;
        private readonly ILogger _logger;
        private readonly IUsageHelpProvider _usageHelpProvider;

        public Replicator(
            IReplicationWorker replicationWorker,
            IConfigResolver configResolver,
            ILogger logger,
            IUsageHelpProvider usageHelpProvider)
        {
            _replicationWorker = replicationWorker;
            _configResolver = configResolver;
            _logger = logger;
            _usageHelpProvider = usageHelpProvider;
        }

        public void Run()
        {
            try
            {
                if (!Utils.IsSingleInstance())
                    throw new UserException(string.Format("Only one instance of {0} is allowed!", Utils.GetAssemblyTitle()));

                var config = _configResolver.GetConfig();
                _replicationWorker.Run(config);
            }
            catch (RouteNotSpecifiedException e)
            {
                OutputHelp(e.Message);
            }
            catch (AggregateException e)
            {
                if (e.Flatten().InnerExceptions.Where(x => x is RouteNotSpecifiedException).Any())
                {
                    OutputHelp(e.Flatten().InnerExceptions.Where(x => x is RouteNotSpecifiedException).First().Message);
                }
                else
                {
                    throw;
                }
            }
        }

        private void OutputHelp(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _logger.LogInfo("-----------------------------");
                _logger.LogInfo(message);
                _logger.LogInfo("-----------------------------");
            }

            _logger.LogInfo(_usageHelpProvider.GetUsageHelp());
        }
    }
}
