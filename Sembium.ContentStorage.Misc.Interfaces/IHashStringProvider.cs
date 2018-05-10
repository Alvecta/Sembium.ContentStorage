using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public interface IHashStringProvider
    {
        string GetHashString(byte[] hash);
        string GetHashString(byte[] hash, int count);

        byte[] GetStringHash(string value);
        (byte[] Hash, int Count) GetStringHashAndCount(string value);
    }
}
