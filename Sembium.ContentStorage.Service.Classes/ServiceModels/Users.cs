using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public class Users : IUsers
    {
        private readonly IContentStorageAccountProvider _contentStorageAccountProvider;

        public Users(
            IContentStorageAccountProvider contentStorageAccountProvider)
        {
            _contentStorageAccountProvider = contentStorageAccountProvider;
        }

        public IEnumerable<IUser> GetUsers(string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            return contentStorageUsersRepository.GetUsers();
        }

        public void AddAdmin(string userName, string userAuthenticationToken, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            contentStorageUsersRepository.AddAdmin(userName, userAuthenticationToken);
        }

        public void AddReplicator(string userName, string userAuthenticationToken, string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            contentStorageUsersRepository.AddReplicator(userName, userAuthenticationToken, containerName);
        }

        public void AddOperator(string userName, string userAuthenticationToken, string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            contentStorageUsersRepository.AddOperator(userName, userAuthenticationToken, containerName);
        }

        public void AddBackupOperator(string userName, string userAuthenticationToken, string containerName, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            contentStorageUsersRepository.AddBackupOperator(userName, userAuthenticationToken, containerName);
        }

        public void DeleteUser(string userAuthenticationToken, string authenticationToken)
        {
            var account = _contentStorageAccountProvider.GetAccount(authenticationToken, null);
            var contentStorageUsersRepository = account.GetContentStorageUsersRepository();

            contentStorageUsersRepository.DeleteUser(userAuthenticationToken);
        }
    }
}