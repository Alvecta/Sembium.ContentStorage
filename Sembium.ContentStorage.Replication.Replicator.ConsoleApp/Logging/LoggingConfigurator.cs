using Microsoft.Extensions.Logging;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Logging
{
    public class LoggingConfigurator : ILoggingConfigurator
    {
        private readonly ILogFileNameProvider _logFileNameProvider;

        public LoggingConfigurator(ILogFileNameProvider logFileNameProvider)
        {
            _logFileNameProvider = logFileNameProvider;
        }

        public void Configure(ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(LogLevel.Information);
            loggerFactory.AddProvider(new ReplicatorConsoleLoggingProvider());

            var logFileName = _logFileNameProvider.GetLogFileName();
            if (!string.IsNullOrEmpty(logFileName))
            {
                loggerFactory.AddSerilog(GetSerilogLoggerConfiguration(logFileName).CreateLogger(), dispose: true);
            }
        }

        private LoggerConfiguration GetSerilogLoggerConfiguration(string logFileName)
        {
            return
                new LoggerConfiguration()
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Verbose)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    logFileName,
                    Serilog.Events.LogEventLevel.Verbose,
                    outputTemplate: "{Timestamp:yyyy-mm-dd HH:mm:ss:fff zzz} {Message:lj} {exception}{NewLine}");
        }
    }
}
