using Sembium.ContentStorage.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public interface IContentStorageUsersRepository
    {
        IEnumerable<IUser> GetUsers();

        void AddAdmin(string userName, string authenticationToken);
        void AddMaintainer(string userName, string authenticationToken);
        void AddReplicator(string userName, string authenticationToken, string containerName);
        void AddBackupOperator(string userName, string authenticationToken, string containerName);
        void AddOperator(string userName, string authenticationToken, string containerName);

        void DeleteUser(string authenticationToken);
    }
}
