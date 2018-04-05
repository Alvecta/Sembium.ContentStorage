using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization
{
    public interface ILoggingConfigurator
    {
        void Configure(ILoggerFactory loggerFactory);
    }
}
