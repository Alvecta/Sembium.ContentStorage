using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public class Sha1HashProvider : IHashProvider
    {
        public byte[] GetHash(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                return sha1.ComputeHash(data);
            }
        }

        public byte[] GetHash(System.IO.Stream stream)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                return sha1.ComputeHash(stream);
            }
        }

        public (byte[] Hash, int Count) GetHashAndCount(IEnumerable<byte[]> datas)
        {
            return GetHashAndCount(datas.Select(x => (x, 1)));
        }

        public (byte[] Hash, int Count) GetHashAndCount(IEnumerable<(byte[], int)> datas)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                var count = 0;

                sha1.Initialize();
                foreach (var data in datas)
                {
                    sha1.TransformBlock(data.Item1, 0, data.Item1.Length, data.Item1, 0);
                    count+= data.Item2;
                }
                sha1.TransformFinalBlock(new byte[0], 0, 0);

                return (sha1.Hash, count);
            }
        }
    }
}
