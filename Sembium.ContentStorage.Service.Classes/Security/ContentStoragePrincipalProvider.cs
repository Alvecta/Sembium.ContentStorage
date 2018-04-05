using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStoragePrincipalProvider : IContentStoragePrincipalProvider
    {
        private readonly IContentStorageIdentityFactory _contentStorageIdentityFactory;
        private readonly IContentStoragePrincipalFactory _contentStoragePrincipalFactory;
        private readonly IContentStorageSystemAccountProvider _contentStorageSystemAccountProvider;

        public ContentStoragePrincipalProvider(
            IContentStorageIdentityFactory contentStorageIdentityFactory,
            IContentStoragePrincipalFactory contentStoragePrincipalFactory,
            IContentStorageSystemAccountProvider contentStorageSystemAccountProvider)
        {
            _contentStorageIdentityFactory = contentStorageIdentityFactory;
            _contentStoragePrincipalFactory = contentStoragePrincipalFactory;
            _contentStorageSystemAccountProvider = contentStorageSystemAccountProvider;
        }

        public IContentStoragePrincipal GetPrincipal(string authenticationToken, string containerName)
        {
            if (string.Equals(containerName, "system", StringComparison.InvariantCultureIgnoreCase))
                return null;

            var users = _contentStorageSystemAccountProvider.GetAccount().GetContentStorageUsersRepository().GetUsers();

            var user =
                users
                .Where(x => x.AuthenticationToken == authenticationToken)
                .Where(x => (x.ContainerName == "*") ||
                            string.Equals(x.ContainerName, containerName, StringComparison.InvariantCultureIgnoreCase))
                .SingleOrDefault();

            return UserToPrincipal(user, containerName);
        }

        private IContentStoragePrincipal UserToPrincipal(IUser user, string containerName)
        {
            if (user == null)
            {
                var identity = _contentStorageIdentityFactory(null, null, false);
                return _contentStoragePrincipalFactory(identity, null);
            }
            else
            {
                var identity = _contentStorageIdentityFactory(user.Name, containerName, true);
                return _contentStoragePrincipalFactory(identity, user.Roles);
            }
        }
    }
}
