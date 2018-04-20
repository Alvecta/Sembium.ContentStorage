using Microsoft.WindowsAzure.Storage.Blob;
using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AzureBlob
{
    public class AzureContentNamesVaultItem : IAzureContentNamesVaultItem
    {
        private readonly CloudAppendBlob _blob;

        public string Name => _blob.Name;

        public AzureContentNamesVaultItem(CloudAppendBlob blob)
        {
            _blob = blob;
        }

        public Stream OpenReadStream()
        {
            return _blob.OpenReadAsync().Result;
        }

        public bool CanAppend(bool compacting)
        {
            _blob.FetchAttributesAsync().Wait();
            return (_blob.Properties.AppendBlobCommittedBlockCount < 49000);
        }

        public void Append(Stream stream)
        {
            if (!_blob.ExistsAsync().Result)
            {
                _blob.CreateOrReplaceAsync().Wait();
            }

            _blob.AppendBlockAsync(stream, null).Wait();
        }

        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            await _blob.DeleteAsync();
        }
    }
}
