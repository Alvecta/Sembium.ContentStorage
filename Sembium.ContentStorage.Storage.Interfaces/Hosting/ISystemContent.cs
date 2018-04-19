using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface ISystemContent : IContent
    {
        Task DeleteAsync(CancellationToken cancellationToken);
    }
}
