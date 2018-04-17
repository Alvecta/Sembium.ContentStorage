using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface IContent
    {
        string Name { get; }
        long GetSize();
        void LoadFromStream(System.IO.Stream stream);
        System.IO.Stream GetReadStream();
    }
}
