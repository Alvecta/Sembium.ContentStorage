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

        public string Name { get; }

        public string ContainerName { get; }

        public bool IsAuthenticated { get; }

        public ContentStorageIdentity(string name, string containerName, bool isAuthenticated)
        {
            Name = name;
            ContainerName = containerName;
            IsAuthenticated = isAuthenticated;
        }
    }
}
