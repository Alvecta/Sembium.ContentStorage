using Sembium.ContentStorage.Service.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public interface IContainerStateRepository
    {
        IEnumerable<IContainerState> GetContainerStates();
        Task<IContainerState> GetContainerStateAsync(string containerName);
        Task SetContainerStateAsync(string containerName, bool? isReadOnly, bool? isMaintained);
    }
}
