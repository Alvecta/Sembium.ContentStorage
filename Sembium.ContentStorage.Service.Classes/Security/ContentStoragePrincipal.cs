using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStoragePrincipal : GenericPrincipal, IContentStoragePrincipal
    {
        public ContentStoragePrincipal(IIdentity identity, IEnumerable<string> roles)
            : base(identity, (roles == null ? null : roles.ToArray()))
        {
        }
    }
}
