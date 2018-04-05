using Sembium.ContentStorage.Replication.Common.Endpoints.Source;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Source
{
    public delegate ISource IFileSystemSourceFactory(string id, IContainer container);
}
