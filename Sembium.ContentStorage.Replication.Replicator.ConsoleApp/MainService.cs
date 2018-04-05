using Sembium.ContentStorage.Replication.Replicator.ConsoleApp.Initialization;
using Sembium.ContentStorage.Replication.Replicator.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.ConsoleApp
{
    public class MainService : IMainService
    {
        private readonly IReplicator _replicator;

        public MainService(IReplicator replicator)
        {
            _replicator = replicator;
        }

        public Task DoMainAsync(string[] args, CancellationToken cancellationToken)
        {
            return Task.Run(() => _replicator.Run());
        }
    }
}
