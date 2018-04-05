using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageSystemPrincipalProvider : IContentStorageSystemPrincipalProvider
    {
        private readonly IContentStorageIdentityFactory _contentStorageIdentityFactory;
        private readonly IContentStoragePrincipalFactory _contentStoragePrincipalFactory;

        public ContentStorageSystemPrincipalProvider(
            IContentStorageIdentityFactory contentStorageIdentityFactory,
            IContentStoragePrincipalFactory contentStoragePrincipalFactory)
        {
            _contentStorageIdentityFactory = contentStorageIdentityFactory;
            _contentStoragePrincipalFactory = contentStoragePrincipalFactory;
        }

        public IContentStoragePrincipal GetPrincipal()
        {
            var identity = _contentStorageIdentityFactory("System", null, true);
            return _contentStoragePrincipalFactory(identity, new[] { Security.Roles.System });
        }
    }
}
