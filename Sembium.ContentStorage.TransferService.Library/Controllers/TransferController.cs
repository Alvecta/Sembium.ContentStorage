using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.FileSystem;
using Sembium.ContentStorage.Storage.FileSystem.Transfer;
using Sembium.ContentStorage.Utils;
using Sembium.ContentStorage.Utils.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.TransferService.Library.Controllers
{
    [Route("[controller]")]
    public class TransferController : Controller
    {
        private const string StorageRootAppSettingName = "StorageRoot";
        private readonly ISignedURLProvider _signedURLProvider;
        private readonly IConfigurationSettings _congigurationSettings;
        private readonly IFileSystemFullFileNameProvider _fileSystemFullFileNameProvider;

        public TransferController(
            ISignedURLProvider signedURLProvider,
            IConfigurationSettings congigurationSettings,
            IFileSystemFullFileNameProvider fileSystemFullFileNameProvider)
        {
            _signedURLProvider = signedURLProvider;
            _congigurationSettings = congigurationSettings;
            _fileSystemFullFileNameProvider = fileSystemFullFileNameProvider;
        }

        private string StorageRoot
        {
            get { return _congigurationSettings.GetAppSetting(StorageRootAppSettingName); }
        }

        private static string GetMimeType(string fileName)
        {
            return MimeKit.MimeTypes.GetMimeType(fileName) ?? MediaTypeNames.Application.Octet;
        }

        [Route("download/{containerName}/{contentName}")]
        [HttpGet]
        public IActionResult Download(string containerName, string contentName, CancellationToken cancellationToken)
        {
            CheckRequest();

            var fullFileName = _fileSystemFullFileNameProvider.GetFullFileName(StorageRoot, containerName, contentName);

            return new FileCallbackResult(
                new MediaTypeHeaderValue(GetMimeType(contentName)),
                async (outputStream, _) =>
                {
                    using (var stream = System.IO.File.OpenRead(fullFileName))
                    {
                        await stream.CopyToParallelAsync(outputStream, 1_000_000, cancellationToken);
                    }
                });
        }

        [Route("upload/{containerName}/{contentName}")]
        [HttpPut]
        //[DisableRequestSizeLimit]
        //[RequestSizeLimit(100_000_000)]
        public async Task Upload(string containerName, string contentName, /*IFormFile file, */CancellationToken cancellationToken)
        {
            CheckRequest();

            var file = HttpContext.Request.Form.Files[0];  // IFormFile file being null fix

            var fullFileName = _fileSystemFullFileNameProvider.GetFullFileName(StorageRoot, containerName, contentName);

            var filePath = System.IO.Path.GetDirectoryName(fullFileName);
            System.IO.Directory.CreateDirectory(filePath);

            using (var formFileStream = file.OpenReadStream())
            {
                using (var fileStream = new System.IO.FileStream(fullFileName, System.IO.FileMode.Create))
                {
                    await formFileStream.CopyToParallelAsync(fileStream, 1_000_000, cancellationToken);
                }
            }
        }

        private void CheckRequest()
        {
            var url = HttpContext.Request.GetDisplayUrl();
            _signedURLProvider.CheckSignedURL(url);
        }
    }
}
