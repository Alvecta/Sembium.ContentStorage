using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class HashStringProvider : IHashStringProvider
    {
        public string GetHashString(byte[] hash)
        {
            return string.Concat(hash.Select(x => x.ToString("X2")));
        }

        public string GetHashString(byte[] hash, int count)
        {
            return string.Format("{0}@{1}", GetHashString(hash), count);
        }

        public byte[] GetStringHash(string value)
        {
            var result = new byte[value.Length / 2];

            for (var i = 0; i < value.Length / 2; i++)
            {
                var byteStr = value.Substring(i * 2, 2);
                result[i] = byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber);
            }

            return result;
        }

        public (byte[] Hash, int Count) GetStringHashAndCount(string value)
        {
            var parts = value.Split('@');

            return (GetStringHash(parts[0]), int.Parse(parts[1]));
        }
    }
}
