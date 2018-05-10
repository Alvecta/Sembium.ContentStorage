using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Logging
{
    public class LoggerLogger : Sembium.ContentStorage.Replication.Logging.ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger<LoggerLogger> _logger;

        public LoggerLogger(Microsoft.Extensions.Logging.ILogger<LoggerLogger> logger)
        {
            _logger = logger;
        }

        public void LogTrace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(string message, Exception exception)
        {
            _logger.LogError(exception, message);
        }

        public void LogFatal(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogFatal(string message, Exception exception)
        {
            _logger.LogCritical(exception, message);
        }
    }
}
