using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Replicator.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Main
{
    public class LoggingReplicator : IReplicator
    {
        private readonly IReplicator _replicator;
        private readonly ILogger _logger;

        public LoggingReplicator(IReplicator replicator, ILogger logger)
        {
            _replicator = replicator;
            _logger = logger;
        }

        public void Run()
        {
            var startTime = DateTimeOffset.Now;
            try
            {
                try
                {
                    try
                    {
                        _logger.LogInfo("Working...");
                        _replicator.Run();
                        _logger.LogInfo("Done.");
                    }
                    catch
                    {
                        _logger.LogInfo("Done with errors. See the log.");
                        throw;
                    }
                }
                catch (AggregateException e)
                {
                    _logger.LogFatal(AggregateExceptionMessage(e), e);
                }
                catch (Exception e)
                {
                    _logger.LogFatal(e.Message, e);
                }
            }
            finally
            {
                _logger.LogInfo("Total time: {0}", DateTimeOffset.Now.Subtract(startTime).ToString());
            }
        }

        private string AggregateExceptionMessage(AggregateException e)
        {
            return string.Join("\n", e.Flatten().InnerExceptions.Select(x => x.Message));
        }
    }
}
