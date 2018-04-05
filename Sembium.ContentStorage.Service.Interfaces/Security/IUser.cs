using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public interface IUser
    {
        string Name { get; }
        string AuthenticationToken { get; }
        string ContainerName { get; }
        IEnumerable<string> Roles { get; }
    }
}
