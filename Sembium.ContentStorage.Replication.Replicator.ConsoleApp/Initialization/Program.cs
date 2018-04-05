using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            int exitCode;

            try
            {
                var serviceProvider = Initializer.GetServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var mainService = serviceProvider.GetService<IMainService>();

                try
                {
                    await mainService.DoMainAsync(args, CancellationToken.None);
                    exitCode = 0;
                }
                catch (Exception e)
                {
                    loggerFactory.CreateLogger("Errors").LogCritical(e, e.Message);
                    exitCode = 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                exitCode = 1;
            }

            #if DEBUG
            Task.Delay(500).Wait();
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
            #endif

            return exitCode;
        }
    }
}
