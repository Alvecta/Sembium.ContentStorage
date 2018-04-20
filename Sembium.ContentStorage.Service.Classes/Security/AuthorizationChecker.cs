using Sembium.ContentStorage.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    class AuthorizationChecker : IAuthorizationChecker
    {
        private readonly IContentStoragePrincipalManager _contentStoragePrincipalManager;

        public AuthorizationChecker(IContentStoragePrincipalManager contentStoragePrincipalManager)
        {
            _contentStoragePrincipalManager = contentStoragePrincipalManager;
        }

        public void CheckUserIsInRole(params string[] roles)
        {
            if (!roles.Any(role => _contentStoragePrincipalManager.Principal.IsInRole(role)))
            {
                throw new UserAuthorizationException("Not authorized");
            }
        }
    }
}
