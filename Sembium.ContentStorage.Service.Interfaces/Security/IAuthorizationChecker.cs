using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Service.Security
{
    public interface IAuthorizationChecker
    {
        void CheckUserIsInRole(params string[] roles);
    }
}
