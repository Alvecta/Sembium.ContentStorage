using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization
{
    public interface IMainService
    {
        Task DoMainAsync(string[] args, CancellationToken cancellationToken);
    }
}
