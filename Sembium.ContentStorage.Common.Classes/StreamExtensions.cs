using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public static class StreamExtensions
    {
        public static IEnumerable<string> ReadAllLines(this System.IO.Stream stream, Encoding encoding)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using (var reader = new System.IO.StreamReader(stream, encoding))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}
