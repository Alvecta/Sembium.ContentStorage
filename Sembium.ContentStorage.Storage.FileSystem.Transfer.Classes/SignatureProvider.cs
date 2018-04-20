using Sembium.ContentStorage.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class SignatureProvider : ISignatureProvider
    {
        private readonly ICertificateProvider _certificateProvider;

        public SignatureProvider(ICertificateProvider certificateProvider)
        {
            _certificateProvider = certificateProvider;
        }

        public string GenerateSignature(string data)
        {
            var signedData = Sign(data);
            return Convert.ToBase64String(signedData);
        }

        public bool VerifySignature(string data, string signature)
        {
            return (!string.IsNullOrEmpty(signature)) && Verify(data, Convert.FromBase64String(signature));
        }

        private byte[] Sign(string data)
        {
            var hash = GetHash(data);
            var rsa = GetRSA(KeyType.Private);

            return rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        private bool Verify(string data, byte[] signature)
        {
            var hash = GetHash(data);
            var rsa = GetRSA(KeyType.Public);

            return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        private static byte[] GetHash(string data)
        {
            var encoding = new UnicodeEncoding();
            var dataBytes = encoding.GetBytes(data);

            var sha1 = new SHA1Managed();

            return sha1.ComputeHash(dataBytes);
        }

        private enum KeyType { Private, Public }

        private RSA GetRSA(KeyType keyType)
        {
            var cert = _certificateProvider.GetCertificate();

            switch (keyType)
            {
                case KeyType.Private:
                    return cert.GetRSAPrivateKey();

                case KeyType.Public:
                    return cert.GetRSAPublicKey();

                default:
                    throw new UserException("Unknown KeyType " + keyType.ToString());
            }
        }
    }
}
