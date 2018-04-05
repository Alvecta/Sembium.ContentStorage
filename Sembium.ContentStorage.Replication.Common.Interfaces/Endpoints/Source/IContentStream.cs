using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Endpoints.Source
{
    public interface IContentStream
    {
        long Size { get; }
        System.IO.Stream Stream { get; }
    }
}
