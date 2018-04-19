using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceModels
{
    public interface IUsers
    {
        IEnumerable<IUser> GetUsers(string authenticationToken);
        void AddAdmin(string userName, string userAuthenticationToken, string authenticationToken);
        void AddReplicator(string userName, string userAuthenticationToken, string containerName, string authenticationToken);
        void AddBackupOperator(string userName, string userAuthenticationToken, string containerName, string authenticationToken);
        void AddOperator(string userName, string userAuthenticationToken, string containerName, string authenticationToken);
        void DeleteUser(string userAuthenticationToken, string authenticationToken);
    }
}
