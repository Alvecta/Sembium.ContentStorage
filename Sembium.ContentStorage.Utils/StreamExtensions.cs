using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Utils
{
    public static class StreamExtensions
    {
        public static async Task CopyToParallelAsync(this Stream source, Stream destination, Int32 bufferSize, CancellationToken cancellationToken)
        {
            Contract.Requires(destination != null);
            Contract.Requires(source.CanRead);
            Contract.Requires(destination.CanWrite);

            await CopyUtils.CopyAsync(
                    (buffer, cancelationToken) => source.ReadAsync(buffer, 0, buffer.Length, cancellationToken),
                    (buffer, count, cancelatlionToken) => destination.WriteAsync(buffer, 0, count, cancellationToken),
                    bufferSize,
                    cancellationToken
                );
        }

        public static IEnumerable<string> ReadAllLines(this System.IO.Stream stream, Encoding encoding)
        {
<<<<<<< HEAD:Sembium.ContentStorage.Utils/StreamExtensions.cs
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
=======
            stream.Position = 0;
>>>>>>> 614029ee3b71460ab6e6035b501da8d4a4705d6b:Sembium.ContentStorage.Utils/StreamExtensions.cs

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
