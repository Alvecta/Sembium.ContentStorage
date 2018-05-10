using Sembium.ContentStorage.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class CertificateStoreCertificateLoader : ICertificateLoader
    {
        private const string CertificateSubjectAppSetting = "CertificateSubject";

        private readonly IConfigurationSettings _configurationSettings;

        public CertificateStoreCertificateLoader(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;
        }

        private string CertificateSubject
        {
            get { return _configurationSettings.GetAppSetting(CertificateSubjectAppSetting); }
        }

        public X509Certificate2 Load()
        {
            if (string.IsNullOrEmpty(CertificateSubject))
                return null;

            using (var store = new X509Store(StoreName.AuthRoot, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                try
                {
                    return
                        store.Certificates.Cast<X509Certificate2>()
                        .Where(c => c.Subject.Contains(CertificateSubject))
                        .FirstOrDefault();
                }
                finally
                {
                    store.Close();
                }
            }
        }
    }
}
