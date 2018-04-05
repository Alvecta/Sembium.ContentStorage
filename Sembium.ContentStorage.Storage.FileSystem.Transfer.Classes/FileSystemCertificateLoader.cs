using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class FileSystemCertificateLoader : ICertificateLoader
    {
        private const string CertificatePathAppSetting = "CertificatePath";
        private const string CertificatePasswordAppSetting = "CertificatePassword";

        private readonly IConfigurationSettings _configurationSettings;

        public FileSystemCertificateLoader(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;
        }

        private string CertificatePath
        {
            get { return _configurationSettings.GetAppSetting(CertificatePathAppSetting); }
        }

        private string CertificatePassword
        {
            get { return _configurationSettings.GetAppSetting(CertificatePasswordAppSetting); }
        }

        public X509Certificate2 Load()
        {
            var certPath = CertificatePath;

            if (string.IsNullOrEmpty(certPath))
                return null;

            if (certPath.Equals(System.IO.Path.GetFileName(certPath), StringComparison.InvariantCultureIgnoreCase))
            {
                var appDataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                certPath = System.IO.Path.Combine(appDataPath, certPath);
            }

            if (!System.IO.File.Exists(certPath))
                return null;

            return LoadCertificate(certPath);
        }

        private X509Certificate2 LoadCertificate(string certPath)
        {
            return new X509Certificate2(certPath, CertificatePassword, X509KeyStorageFlags.MachineKeySet);
        }
    }
}
