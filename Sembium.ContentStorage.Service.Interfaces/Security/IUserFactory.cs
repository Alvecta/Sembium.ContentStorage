using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public delegate IUser IUserFactory(string name, string authenticationToken, string containerName, IEnumerable<string> roles);
}
