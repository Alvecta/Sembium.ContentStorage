using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public delegate IContentNamesVaultItem IAzureContentNamesVaultItemFactory(Microsoft.WindowsAzure.Storage.Blob.CloudAppendBlob blob);
}
