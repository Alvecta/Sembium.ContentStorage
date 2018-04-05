using Sembium.ContentStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class UserAuthorizationException : UserException
    {
        public UserAuthorizationException(string message) : base(message)
        {
        }
    }
}
