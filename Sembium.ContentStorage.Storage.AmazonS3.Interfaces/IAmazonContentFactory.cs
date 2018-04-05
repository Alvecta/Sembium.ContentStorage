using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public delegate IContent IAmazonContentFactory(string bucketName, string keyName);
}
