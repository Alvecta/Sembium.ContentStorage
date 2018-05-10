using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public interface IHashProvider
    {
        byte[] GetHash(byte[] data);
        byte[] GetHash(System.IO.Stream stream);
        (byte[] Hash, int Count) GetHashAndCount(IEnumerable<byte[]> datas);
        (byte[] Hash, int Count) GetHashAndCount(IEnumerable<(byte[], int)> datas);
    }
}
