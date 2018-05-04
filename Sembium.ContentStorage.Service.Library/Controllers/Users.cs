using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Service.ServiceModels;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Utils;
using Sembium.ContentStorage.Utils.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Library.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsers _users;

        public UsersController(IUsers users)
        {
            _users = users;
        }

        private string FixContainerName(string containerName)
        {
            if (string.Equals(containerName, "x"))
                return "*"; // * not allowed in url path

            return containerName;
        }

        /// <summary>
        /// Gets users list
        /// </summary>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of users</returns>
        [Route("")]
        [HttpGet]
        public IEnumerable<IUser> GetUsers([FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _users.GetUsers(auth ?? authenticationKey);
        }

        /// <summary>
        /// Adds new admin user
        /// </summary>
        /// <param name="userName">Admin user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("admins")]
        [HttpPut]
        public void AddAdmin([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.AddAdmin(userName, userAuth, auth ?? authenticationKey);
        }

        /// <summary>
        /// Adds new maintainer user
        /// </summary>
        /// <param name="userName">Maintainer user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("maintainers")]
        [HttpPut]
        public void AddMaintainer([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.AddMaintainer(userName, userAuth, auth ?? authenticationKey);
        }

        /// <summary>
        /// Adds new replicator user
        /// </summary>
        /// <param name="userName">Replicator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("replicators")]
        [HttpPut]
        public void AddReplicator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.AddReplicator(userName, userAuth, FixContainerName(container), auth ?? authenticationKey);
        }

        /// <summary>
        /// Adds new operator user
        /// </summary>
        /// <param name="userName">Operator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("operators")]
        [HttpPut]
        public void AddOperator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.AddOperator(userName, userAuth, FixContainerName(container), auth ?? authenticationKey);
        }

        /// <summary>
        /// Adds new backup operator user
        /// </summary>
        /// <param name="userName">Backup operator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("backupops")]
        [HttpPut]
        public void AddBackupOperator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.AddBackupOperator(userName, userAuth, FixContainerName(container), auth ?? authenticationKey);
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="userAuth">Authentication token of the user to be deleted</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("")]
        [HttpDelete]
        public void DeleteUser([FromQuery]string userAuth, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            _users.DeleteUser(userAuth, auth ?? authenticationKey);
        }
    }
}
