using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Utils
{
    class ReplicatorConsoleLoggingProvider : ILoggerProvider
    {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return new ReplicatorConsoleLogger(categoryName);
        }

        public class ReplicatorConsoleLogger : ILogger
        {
            private readonly string _categoryName;

            public ReplicatorConsoleLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                Console.WriteLine($"{formatter(state, exception)}");

                if (exception != null)
                {
                    //Console.WriteLine(exception.StackTrace.ToString());
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return (logLevel >= LogLevel.Information);
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
