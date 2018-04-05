using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public delegate IContentStream IContentStreamFactory(long size, System.IO.Stream stream);
}
