using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sembium.ContentStorage.Client;

namespace Sembium.ContentStorage.Uploader.Library.Controllers
{
    [Route("[controller]")]
    public class UploadController : Controller
    {
        private readonly IContentStorageUploader _contentStorageUploader;

        public UploadController(IContentStorageUploader contentStorageUploader)
        {
            _contentStorageUploader = contentStorageUploader;
        }

        [HttpPost]
        public void Post(
            [FromHeader(Name = "Uploader-ContentUrl")]                  string contentUrl,
            [FromHeader(Name = "Uploader-ContentStorageServiceUrl")]    string contentStorageServiceUrl,
            [FromHeader(Name = "Uploader-ContainerName")]               string containerName, 
            [FromHeader(Name = "Uploader-ContentID")]                   string contentID, 
            [FromHeader(Name = "Uploader-Size")]                        string size, 
            [FromHeader(Name = "Uploader-AuthenticationToken")]         string authenticationToken)
        {
            _contentStorageUploader.UploadContent(contentUrl, contentStorageServiceUrl, containerName, contentID, long.Parse(size), authenticationToken);
        }
    }
}
