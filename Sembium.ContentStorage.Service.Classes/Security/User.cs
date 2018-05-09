using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class User : IUser
    {
        public string Name { get; }
        public string AuthenticationToken { get; }
        public string ContainerName { get; }
        public IEnumerable<string> Roles { get; }

        private User()
        {
            // do nothing
        }

        public User(string name, string authenticationToken, string containerName, IEnumerable<string> roles)
        {
            Name = name;
            AuthenticationToken = authenticationToken;
            ContainerName = containerName;
            Roles = roles;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IUser;

            return
                (other != null) &&
                (Name == other.Name) &&
                (AuthenticationToken == other.AuthenticationToken) &&
                (ContainerName == other.ContainerName) &&
                Roles.SequenceEqual(other.Roles);
        }

        private int StringLength(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            return value.Length;
        }

        public override int GetHashCode()
        {
            return StringLength(Name) + StringLength(AuthenticationToken) + StringLength(ContainerName) + Roles.Count();
        }
    }
}
