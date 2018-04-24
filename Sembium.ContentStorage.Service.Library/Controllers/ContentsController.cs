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
    public class ContentsController : Controller
    {
        private readonly IContents _contents;

        public ContentsController(IContents contents)
        {
            _contents = contents;
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
        public IDocumentIDUploadInfo GetUploadInfoOrDocumentID(string containerName, [FromQuery]string hash, [FromQuery]string ext, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetIDUploadInfoOrDocumentID(containerName, hash, ext, auth ?? authenticationKey);
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
        public IIDUploadInfo GetUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetIDUploadInfo(containerName, contentID, auth ?? authenticationKey);
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
        public async Task<string> CommitUploadAsync(string containerName, [FromQuery]string uploadID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return await _contents.CommitUploadAsync(containerName, uploadID, auth ?? authenticationKey);
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
        public string GetDocumentDownloadUrl(string containerName, [FromQuery]string documentID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetDocumentDownloadUrl(containerName, documentID, auth ?? authenticationKey);
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
        public string GetContentDownloadUrl(string containerName, [FromQuery]string contentID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetContentDownloadUrl(containerName, contentID, auth ?? authenticationKey);
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
        public IActionResult GetContentIDs(string containerName, [FromQuery]DateTimeOffset after, [FromQuery]string auth, [FromHeader]string authenticationKey, CancellationToken cancellationToken)
        {
            return new FileCallbackResult(
                new MediaTypeHeaderValue("text/plain"),
                async (outputStream, _) =>
                {
                    using (var contentIDsEnumerator = _contents.GetContentIDs(containerName, after, auth ?? authenticationKey).GetEnumerator())
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
        public int GetContentCount(string containerName, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetContentCount(containerName, auth ?? authenticationKey);
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
        public string GetContentsHash(string containerName, [FromQuery]DateTimeOffset before, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetContentsHash(containerName, before, auth ?? authenticationKey);
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
        public IDocumentMultiPartIDUploadInfo GetMultiPartUploadInfoOrDocumentID(string containerName, [FromQuery]string hash, [FromQuery]string ext, [FromQuery]long size, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetMultiPartIDUploadInfoOrDocumentID(containerName, hash, ext, size, auth ?? authenticationKey);
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
        public IMultiPartIDUploadInfo GetMultiPartUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]long size, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetMultiPartIDUploadInfo(containerName, contentID, size, auth ?? authenticationKey);
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
        public async Task<string> CommitMultiPartUploadAsync(string containerName, [FromBody] CommitMultiPartUploadParams commitMultiPartUploadParams, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return await _contents.CommitMultiPartUploadAsync(containerName, commitMultiPartUploadParams.UploadID, commitMultiPartUploadParams.PartUploadResults, auth ?? authenticationKey);
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
        public IDownloadInfo GetDocumentDownloadInfo(string containerName, [FromQuery]string documentID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetDocumentDownloadInfo(containerName, documentID, auth ?? authenticationKey);
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
        public IDownloadInfo GetContentDownloadInfo(string containerName, [FromQuery]string contentID, [FromQuery]string auth, [FromHeader]string authenticationKey)
        {
            return _contents.GetContentDownloadInfo(containerName, contentID, auth ?? authenticationKey);
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
        public IHttpRequestInfo GetUrlContentUploadInfo(string containerName, [FromQuery]string contentID, [FromQuery]long size, [FromQuery]string auth, [FromHeader]string authenticationKey,
            [FromHeader]string contentUrl, [FromHeader]string contentStorageServiceUrl)
        {
            return _contents.GetUrlContentUploadInfo(contentUrl, contentStorageServiceUrl, containerName, contentID, size, auth ?? authenticationKey);
        }
    }
}
