using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class CachedCertificateProvider : ICertificateProvider
    {
        private readonly ICertificateProvider _certificateProvider;

        private X509Certificate2 cache;

        public CachedCertificateProvider(ICertificateProvider certificateProvider)
        {
            _certificateProvider = certificateProvider;
        }

        public X509Certificate2 GetCertificate()
        {
            if (cache == null)
            {
                cache = _certificateProvider.GetCertificate();
            }

            return cache;
        }
    }
}
