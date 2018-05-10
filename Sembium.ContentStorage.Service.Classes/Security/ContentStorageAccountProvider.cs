using Sembium.ContentStorage.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageAccountProvider : IContentStorageAccountProvider
    {
        private const int InvalidCredentialsSleepMiliseconds = 1000;

        private readonly IContentStorageAccountFactory _contentStorageAccountFactory;
        private readonly IContentStoragePrincipalProvider _contentStoragePrincipalProvider;
        private readonly IContentStoragePrincipalManager _contentStoragePrincipalManager;

        public ContentStorageAccountProvider(
            IContentStorageAccountFactory contentStorageAccountFactory,
            IContentStoragePrincipalProvider contentStoragePrincipalProvider,
            IContentStoragePrincipalManager contentStoragePrincipalManager)
        {
            _contentStorageAccountFactory = contentStorageAccountFactory;
            _contentStoragePrincipalProvider = contentStoragePrincipalProvider;
            _contentStoragePrincipalManager = contentStoragePrincipalManager;
        }

        public IContentStorageAccount GetAccount(string authenticationToken, string containerName)
        {
            var principal = _contentStoragePrincipalProvider.GetPrincipal(authenticationToken, containerName);

            if (!principal.Identity.IsAuthenticated)
                InvalidCredentialsError();

            _contentStoragePrincipalManager.Principal = principal;

            return _contentStorageAccountFactory();
        }

        private void InvalidCredentialsError()
        {
            Task.Delay(InvalidCredentialsSleepMiliseconds).Wait();
            throw new UserAuthenticationException("Invalid credentials");
        }
    }
}
