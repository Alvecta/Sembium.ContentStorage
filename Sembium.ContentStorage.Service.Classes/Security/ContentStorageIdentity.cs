using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageIdentity : IContentStorageIdentity
    {
        public string AuthenticationType
        {
            get { return "Membership"; }
        }

        public string Name { get; private set; }

        public string ContainerName { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public ContentStorageIdentity(string name, string containerName, bool isAuthenticated)
        {
            Name = name;
            ContainerName = containerName;
            IsAuthenticated = isAuthenticated;
        }
    }
}
