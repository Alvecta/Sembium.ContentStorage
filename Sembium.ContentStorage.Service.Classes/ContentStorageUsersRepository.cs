using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Service.Hosting;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public class ContentStorageUsersRepository : IContentStorageUsersRepository
    {
        private const string UsersContentName = "users.json";

        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IUserFactory _userFactory;
        private readonly ISerializer _serializer;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IAuthorizationChecker _authorizationChecker;

        public ContentStorageUsersRepository(
            ISystemContainerProvider systemContainerProvider,
            IUserFactory userFactory,
            ISerializer serializer,
            IContentIdentifierGenerator contentIdentifierGenerator,
            IAuthorizationChecker authorizationChecker)
        {
            _systemContainerProvider = systemContainerProvider;
            _userFactory = userFactory;
            _serializer = serializer;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _authorizationChecker = authorizationChecker;
        }

        public void AddAdmin(string userName, string authenticationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin, Security.Roles.System);

            AddUser(userName, authenticationToken, "*", Security.Roles.Admin, Security.Roles.Replicator, Security.Roles.Backup, Security.Roles.Operator);
        }

        public void AddReplicator(string userName, string authenticationToken, string containerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            AddUser(userName, authenticationToken, containerName, Security.Roles.Replicator);
        }

        public void AddBackupOperator(string userName, string authenticationToken, string containerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            AddUser(userName, authenticationToken, containerName, Security.Roles.Backup);
        }

        public void AddOperator(string userName, string authenticationToken, string containerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            AddUser(userName, authenticationToken, containerName, Security.Roles.Operator);
        }

        private void AddUser(string name, string authenticationToken, string containerName, params string[] roles)
        {
            var newUser = _userFactory(name, authenticationToken, containerName, roles);
            var users = LoadUsers();

            if (users.Any(x => (x.Name == newUser.Name) || (x.AuthenticationToken == newUser.AuthenticationToken)))
                throw new UserException("Duplicate user name or authentication token.");

            var newUsers = users.Concat(new[] { newUser });

            SaveUsers(newUsers);
        }

        public void DeleteUser(string authenticationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            var users = LoadUsers();

            var user = users.Where(x => x.AuthenticationToken == authenticationToken).SingleOrDefault();

            if (user == null)
                throw new UserException("User with specified authentication token does not exist.");

            var newUsers = users.Except(new[] { user });

            SaveUsers(newUsers);
        }

        public IEnumerable<IUser> GetUsers()
        {
            return LoadUsers();
        }

        /// <summary>
        /// Concurency unsafe
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IUser> LoadUsers()
        {
            var usersContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(UsersContentName);
            var serializedUsers = _systemContainerProvider.GetSystemContainer().GetStringContent(usersContentIdentifier);

            if (string.IsNullOrEmpty(serializedUsers))
                return InitialUsers();

            return _serializer.Deserialize<IEnumerable<User>>(serializedUsers);
        }

        // generator: https://www.grc.com/passwords.htm
        private IEnumerable<IUser> InitialUsers()
        {
            return new[] { _userFactory("initadmin", "initadmin", null, new[] { Security.Roles.Admin }) };
        }

        private void SaveUsers(IEnumerable<IUser> users)
        {
            users = users.Except(InitialUsers());

            if (users.Any() && (!users.Any(x => x.Roles.Contains(Security.Roles.Admin))))
                throw new UserException("At least one user with 'admin' role required!");

            var serializedUsers = _serializer.Serialize(users);

            var usersContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(UsersContentName);

            _systemContainerProvider.GetSystemContainer().SetStringContent(usersContentIdentifier, serializedUsers);
        }
    }
}
