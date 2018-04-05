using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public delegate IContentStorageIdentity IContentStorageIdentityFactory(string name, string containerName, bool isAuthenticated);
}
