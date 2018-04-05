using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Common.Exceptions
{
    public class RouteNotSpecifiedException : Exception
    {
        public RouteNotSpecifiedException(string message = null)
            : base(message)
        {
        }
    }
}
