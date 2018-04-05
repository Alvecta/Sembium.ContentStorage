using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public delegate IContainer IAzureContainerFactory(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer delegateContainer);
}
