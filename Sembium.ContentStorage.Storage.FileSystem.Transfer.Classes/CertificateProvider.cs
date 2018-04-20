using Sembium.ContentStorage.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class CertificateProvider : ICertificateProvider
    {
        private readonly IEnumerable<ICertificateLoader> _certificateLoaders;

        public CertificateProvider(IEnumerable<ICertificateLoader> certificateLoaders)
        {
            _certificateLoaders = certificateLoaders;
        }

        public X509Certificate2 GetCertificate()
        {
            var result = _certificateLoaders.Select(x => x.Load()).Where(x => x != null).FirstOrDefault();

            if (result == null)
                throw new UserException("No valid cert was found");

            return result;
        }
    }
}
