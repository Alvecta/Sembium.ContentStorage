using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public delegate IContentStoragePrincipal IContentStoragePrincipalFactory(IContentStorageIdentity identity, IEnumerable<string> roles);
}
