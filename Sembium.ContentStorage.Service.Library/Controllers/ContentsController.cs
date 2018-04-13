using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Service.ServiceModels;
using Sembium.ContentStorage.Service.Library.Utils;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Library.Controllers
{
    [Route("[controller]")]
    public class ContentsController : Controller
    {
        private readonly IContents _contents;

        public ContentsController(IContents contents)
        {
            _contents = contents;
        }

        [Route("containers")]
        [HttpGet]
        public IEnumerable<string> GetContainerNames([FromQuery]string auth)
        {
            return _contents.GetContainerNames(auth);
        }

        /// <summary>
        /// Adds a container
        /// </summary>
        /// <param name="containerName">Name of the container to be created</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("containers/add/{containerName}")]
        [HttpPut]
        public void CreateContainer(string containerName, [FromQuery]string auth)
        {
            _contents.CreateContainer(containerName, auth);
        }
        
        /// <summary>
        /// Maintains a container
        /// </summary>
        /// <param name="containerName">Name of the container to be maintain</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>HTTP response code</returns>
        [Route("containers/{containerName}/maintain")]
        [HttpPut]
        public async Task<string> MaintainContainerAsync(string containerName, [FromQuery]string auth, CancellationToken cancellationToken)
        {
            return await _contents.MaintainContainerAsync(containerName, auth, cancellationToken);
        }

        /// <summary>
        /// Maintains all containers
        /// </summary>
        /// <param name="auth">Authentication token for the request</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>HTTP response code</returns>
        [Route("maintain")]
        [HttpPut]
        public IActionResult Maintain([FromQuery]string auth, CancellationToken cancellationToken)
        {
            //return _contents.Maintain(auth, cancellationToken);

            return new FileCallbackResult(
                new MediaTypeHeaderValue("text/plain"),
                async (outputStream, _) =>
                {
                    var messages = _contents.Maintain(auth, cancellationToken);

                    foreach (var message in messages)
                    {
                        var buffer = Encoding.UTF8.GetBytes(message + Environment.NewLine);
                        await outputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    }
                });
        }

        /// <summary>
        /// Gets list of the container states
        /// </summary>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of strings</returns>
        [Route("containers/states")]
        [HttpGet]
        public IEnumerable<IContainerState> GetContainerStates([FromQuery]string auth)
        {
            return _contents.GetContainerStates(auth);
        }

        /// <summary>
        /// Gets list of the readonly containers in the system
        /// </summary>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of strings</returns>
        [Route("readonlycontainers")]
        [HttpGet]
        public IEnumerable<string> GetReadOnlyContainerNames([FromQuery]string auth)
        {
            return _contents.GetReadOnlyContainerNames(auth);
        }

        /// <summary>
        /// Gets list of the readonly containers in the system
        /// </summary>
        /// <param name="containerName">Name of the container to be made readonly</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of strings</returns>
        [Route("readonlycontainers/add/{containerName}")]
        [HttpPut]
        public void SetContainerReadOnly(string containerName, [FromQuery]string auth)
        {
            _contents.SetContainerReadOnlyState(containerName, true, auth);
        }

        /// <summary>
        /// Gets list of the readonly containers in the system
        /// </summary>
        /// <param name="containerName">Name of the container to be made readonly</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of strings</returns>
        [Route("readonlycontainers/delete/{containerName}")]
        [HttpDelete]
        public void SetContainerNotReadOnly(string containerName, [FromQuery]string auth)
        {
            _contents.SetContainerReadOnlyState(containerName, false, auth);
        }

        /// <summary>
        /// Gets users list
        /// </summary>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>List of users</returns>
        [Route("users")]
        [HttpGet]
        public IEnumerable<IUser> GetUsers([FromQuery]string auth)
        {
            return _contents.GetUsers(auth);
        }

        /// <summary>
        /// Adds new admin user
        /// </summary>
        /// <param name="userName">Admin user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("users/admins")]
        [HttpPut]
        public void AddAdmin([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string auth)
        {
            _contents.AddAdmin(userName, userAuth, auth);
        }

        private string FixContainerName(string containerName)
        {
            if (string.Equals(containerName, "x"))
                return "*"; // * not allowed in url path

            return containerName;
        }

        /// <summary>
        /// Adds new replicator user
        /// </summary>
        /// <param name="userName">Replicator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("users/replicators")]
        [HttpPut]
        public void AddReplicator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth)
        {
            _contents.AddReplicator(userName, userAuth, FixContainerName(container), auth);
        }

        /// <summary>
        /// Adds new operator user
        /// </summary>
        /// <param name="userName">Operator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("users/operators")]
        [HttpPut]
        public void AddOperator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth)
        {
            _contents.AddOperator(userName, userAuth, FixContainerName(container), auth);
        }

        /// <summary>
        /// Adds new backup operator user
        /// </summary>
        /// <param name="userName">Backup operator user name</param>
        /// <param name="userAuth">Authentication token for the user</param>
        /// <param name="container">Name of the container granted to the user. Use "x" to grant all containers to the user.</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("users/backupops")]
        [HttpPut]
        public void AddBackupOperator([FromQuery]string userName, [FromQuery]string userAuth, [FromQuery]string container, [FromQuery]string auth)
        {
            _contents.AddBackupOperator(userName, userAuth, FixContainerName(container), auth);
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="userAuth">Authentication token of the user to be deleted</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response</returns>
        [Route("users/delete")]
        [HttpDelete]
        public void DeleteUser([FromQuery]string userAuth, [FromQuery]string auth)
        {
            _contents.DeleteUser(userAuth, auth);
        }

        /// <summary>
        /// Gets upload info for a document
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="hash">Hash of the content</param>
        /// <param name="ext">Extensions of the content file name</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Document upload info. No upload URL is returned if the document already exists.</returns>
        [Route("GetUploadInfoOrDocumentID/{containerName}")]
        [HttpGet]
        public IDocumentIDUploadInfo GetUploadInfoOrDocumentID(string containerName, [FromQuery]string hash, [FromQuery]string ext, [FromQuery]string auth)
        {
            return _contents.GetIDUploadInfoOrDocumentID(containerName, hash, ext, auth);
        }

        /// <summary>
        /// Gets upload info for a content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="contentID">ContentID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Content upload info. No upload URL is returned if the content already exists.</returns>
        [Route("GetUploadInfo/{containerName}")]
        [HttpGet]
        public IIDUploadInfo GetUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]string auth)
        {
            return _contents.GetIDUploadInfo(containerName, contentID, auth);
        }

        /// <summary>
        /// Commits uploaded document or content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="uploadID">uploadID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Committed DocumentID</returns>
        [Route("CommitUpload/{containerName}")]
        [HttpGet]
        public async Task<string> CommitUploadAsync(string containerName, [FromQuery]string uploadID, [FromQuery]string auth)
        {
            return await _contents.CommitUploadAsync(containerName, uploadID, auth);
        }

        /// <summary>
        /// Gets download URL for a document
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="documentID">DocumentID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>URL</returns>
        [Route("GetDocumentDownloadUrl/{containerName}")]
        [HttpGet]
        public string GetDocumentDownloadUrl(string containerName, [FromQuery]string documentID, [FromQuery]string auth)
        {
            return _contents.GetDocumentDownloadUrl(containerName, documentID, auth);
        }

        /// <summary>
        /// Gets download URL for a content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="contentID">ContentID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>URL</returns>
        [Route("GetContentDownloadUrl/{containerName}")]
        [HttpGet]
        public string GetContentDownloadUrl(string containerName, [FromQuery]string contentID, [FromQuery]string auth)
        {
            return _contents.GetContentDownloadUrl(containerName, contentID, auth);
        }

        /// <summary>
        /// Gets list of content identifiers in a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="after">Minimum UTC for the contents to be returned</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>IEnumerable of string</returns>
        [Route("containers/{containerName}")]
        [HttpGet]
        public IActionResult GetContentIDs(string containerName, [FromQuery]DateTimeOffset after, [FromQuery]string auth, CancellationToken cancellationToken)
        {
            return new FileCallbackResult(
                new MediaTypeHeaderValue("text/plain"),
                async (outputStream, _) =>
                {
                    using (var contentIDsEnumerator = _contents.GetContentIDs(containerName, after, auth).GetEnumerator())
                    {
                        await CopyUtils.CopyAsync(
                                (buffer, ct) =>
                                {
                                    var byteCount = 0;
                                    while ((byteCount < buffer.Length - 1000) && (contentIDsEnumerator.MoveNext()))
                                    {
                                        var enumBuff = Encoding.UTF8.GetBytes(contentIDsEnumerator.Current + Environment.NewLine);
                                        enumBuff.CopyTo(buffer, byteCount);
                                        byteCount += enumBuff.Length;
                                    }

                                    return Task.FromResult(byteCount);
                                },
                                async (buffer, count, cancellationToken2) =>
                                {
                                    await outputStream.WriteAsync(buffer, 0, count, cancellationToken2);
                                },
                                1_000_000,
                                cancellationToken
                            );
                    }
                });
        }

        /// <summary>
        /// Gets count of the contents in a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Number</returns>
        [Route("containers/{containerName}/count")]
        [HttpGet]
        public int GetContentCount(string containerName, [FromQuery]string auth)
        {
            return _contents.GetContentCount(containerName, auth);
        }

        /// <summary>
        /// Gets hash of content hashes in a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="before">Maximum UTC for the contents to be hashed</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>A string value representing the hash of content hashes in the container</returns>
        [Route("containers/{containerName}/hash")]
        [HttpGet]
        public string GetContentsHash(string containerName, [FromQuery]DateTimeOffset before, [FromQuery]string auth)
        {
            return _contents.GetContentsHash(containerName, before, auth);
        }

        /// <summary>
        /// Gets readonly subcontainer names from a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("containers/{containerName}/subcontainers")]
        [HttpGet]
        public IEnumerable<string> GetContainerSubcontainers(string containerName, [FromQuery]string auth)
        {
            return _contents.GetContainerReadOnlySubcontainerNames(containerName, auth);
        }

        /// <summary>
        /// Adds new readonly subcontainer to a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="subcontainerName">Read only subcontainer name</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("containers/{containerName}/subcontainers/add/{subcontainerName}")]
        [HttpPut]
        public void AddContainerSubcontainer(string containerName, string subcontainerName, [FromQuery]string auth)
        {
            _contents.AddContainerReadOnlySubcontainer(containerName, subcontainerName, auth);
        }

        /// <summary>
        /// Removes a readonly subcontainer from a container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="subcontainerName">Read only subcontainer name</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>HTTP response code</returns>
        [Route("containers/{containerName}/subcontainers/delete/{subcontainerName}")]
        [HttpDelete]
        public void RemoveContainerSubcontainer(string containerName, string subcontainerName, [FromQuery]string auth)
        {
            _contents.RemoveContainerReadOnlySubcontainer(containerName, subcontainerName, auth);
        }

        /// <summary>
        /// Gets multipart upload info for a document
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="hash">Hash of the content</param>
        /// <param name="ext">Extensions of the content file name</param>
        /// <param name="size">Size in bytes of the content</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Document upload info. No multipart upload info is returned if the document already exists.</returns>
        [Route("GetMultiPartUploadInfoOrDocumentID/{containerName}")]
        [HttpGet]
        public IDocumentMultiPartIDUploadInfo GetMultiPartUploadInfoOrDocumentID(string containerName, [FromQuery]string hash, [FromQuery]string ext, [FromQuery]long size, [FromQuery]string auth)
        {
            return _contents.GetMultiPartIDUploadInfoOrDocumentID(containerName, hash, ext, size, auth);
        }

        /// <summary>
        /// Gets multipart upload info for a content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="contentID">ContentID</param>
        /// <param name="size">Size in bytes of the content</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Content upload info. No upload info is returned if the content already exists.</returns>
        [Route("GetMultiPartUploadInfo/{containerName}")]
        [HttpGet]
        public IMultiPartIDUploadInfo GetMultiPartUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]long size, [FromQuery]string auth)
        {
            return _contents.GetMultiPartIDUploadInfo(containerName, contentID, size, auth);
        }

        public class CommitMultiPartUploadParams
        {
            public string UploadID { get; set; }
            public IEnumerable<KeyValuePair<string, string>> PartUploadResults { get; set; }
        }

        /// <summary>
        /// Commits multipart uploaded document or content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="commitMultiPartUploadParams">parameters from the body</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Committed DocumentID</returns>
        [Route("CommitMultiPartUpload/{containerName}")]
        [HttpPost]
        public async Task<string> CommitMultiPartUploadAsync(string containerName, [FromBody] CommitMultiPartUploadParams commitMultiPartUploadParams, [FromQuery]string auth)
        {
            return await _contents.CommitMultiPartUploadAsync(containerName, commitMultiPartUploadParams.UploadID, commitMultiPartUploadParams.PartUploadResults, auth);
        }

        /// <summary>
        /// Gets download info for a document
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="documentID">DocumentID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Download info</returns>
        [Route("GetDocumentDownloadInfo/{containerName}")]
        [HttpGet]
        public IDownloadInfo GetDocumentDownloadInfo(string containerName, [FromQuery]string documentID, [FromQuery]string auth)
        {
            return _contents.GetDocumentDownloadInfo(containerName, documentID, auth);
        }

        /// <summary>
        /// Gets download info for a content
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="contentID">ContentID</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Download info</returns>
        [Route("GetContentDownloadInfo/{containerName}")]
        [HttpGet]
        public IDownloadInfo GetContentDownloadInfo(string containerName, [FromQuery]string contentID, [FromQuery]string auth)
        {
            return _contents.GetContentDownloadInfo(containerName, contentID, auth);
        }

        /// <summary>
        /// Gets upload info for url content upload
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="contentID">ContentID</param>
        /// <param name="size">Size in bytes of the content</param>
        /// <param name="auth">Authentication token for the request</param>
        /// <returns>Document upload info. No multipart upload info is returned if the document already exists.</returns>
        [Route("GetUrlContentUploadInfo/{containerName}")]
        [HttpGet]
        public IHttpRequestInfo GetUrlContentUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]long size, [FromQuery]string auth,
            [FromHeader]string contentUrl, [FromHeader]string contentStorageServiceUrl)
        {
            return _contents.GetUrlContentUploadInfo(contentUrl, contentStorageServiceUrl, containerName, contentID, size, auth);
        }
    }
}
