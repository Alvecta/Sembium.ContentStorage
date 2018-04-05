using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class Sha1Provider : IHashProvider
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
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                var count = 0;

                sha1.Initialize();
                foreach (var data in datas)
                {
                    sha1.TransformBlock(data, 0, data.Length, data, 0);
                    count++;
                }
                sha1.TransformFinalBlock(new byte[0], 0, 0);

                return (sha1.Hash, count);
            }
        }
    }
}
