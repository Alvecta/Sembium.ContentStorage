using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public interface ISignatureProvider
    {
        string GenerateSignature(string data);
        bool VerifySignature(string data, string signature);
    }
}
