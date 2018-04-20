using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageAccount : IContentStorageAccount
    {
        private readonly IContentStorageFactory _contentStorageFactory;
        private readonly IContentStorageContainerFactory _contentStorageContainerFactory;
        private readonly IContentStorageUsersRepositoryFactory _contentStorageUsersRepositoryFactory;
        private readonly IAuthorizationChecker _authorizationChecker;

        public ContentStorageAccount(
            IContentStorageFactory contentStorageFactory,
            IContentStorageContainerFactory contentStorageContainerFactory,
            IContentStorageUsersRepositoryFactory contentStorageUsersRepositoryFactory,
            IAuthorizationChecker authorizationChecker)
        {
            _contentStorageFactory = contentStorageFactory;
            _contentStorageContainerFactory = contentStorageContainerFactory;
            _contentStorageUsersRepositoryFactory = contentStorageUsersRepositoryFactory;
            _authorizationChecker = authorizationChecker;
        }

        public IContentStorageContainer GetContentStorageContainer()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.Maintainer, Security.Roles.Replicator, Security.Roles.Operator, Security.Roles.Backup);

            return _contentStorageContainerFactory(((IContentStorageIdentity)Thread.CurrentPrincipal.Identity).ContainerName);
        }

        public IContentStorage GetContentStorage()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.Maintainer, Security.Roles.System, Security.Roles.Replicator);

            return _contentStorageFactory();
        }

        public IContentStorageUsersRepository GetContentStorageUsersRepository()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.System);

            return _contentStorageUsersRepositoryFactory();
        }
    }
}
