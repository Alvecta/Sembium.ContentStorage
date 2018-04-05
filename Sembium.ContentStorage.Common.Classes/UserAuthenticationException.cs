using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public class UserAuthenticationException : UserException
    {
        public UserAuthenticationException(string message) : base(message)
        {
        }
    }
}
