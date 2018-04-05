using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sembium.ContentStorage.Client
{
    public interface IContentStorageUploader
    {
        void UploadContent(Stream stream, string contentStorageServiceURL, string containerName, IContentIdentifier contentIdentifier, long size, string authenticationToken);
        void UploadContent(Stream stream, string contentStorageServiceURL, string containerName, string contentID, long size, string authenticationToken);
        void UploadContent(string contentUrl, string contentStorageServiceURL, string containerName, string contentID, long size, string authenticationToken);
    }
}
