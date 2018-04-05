using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface IHashProvider
    {
        byte[] GetHash(byte[] data);
        byte[] GetHash(System.IO.Stream stream);
        (byte[] Hash, int Count) GetHashAndCount(IEnumerable<byte[]> datas);
    }
}
