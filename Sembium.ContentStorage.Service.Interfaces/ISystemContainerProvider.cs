using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public interface ISystemContainerProvider
    {
        ISystemContainer GetSystemContainer(string specificName = null);
    }
}
