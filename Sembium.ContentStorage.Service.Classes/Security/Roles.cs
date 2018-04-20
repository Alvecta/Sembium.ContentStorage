using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public static class Roles
    {
        public const string System = "system";
        public const string Admin = "admin";
        public const string Maintainer = "maintainer";
        public const string Backup = "backup";
        public const string Operator = "operator";
        public const string Replicator = "replicator";
    }
}
