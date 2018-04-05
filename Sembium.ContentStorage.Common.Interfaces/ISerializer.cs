using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string input);
    }
}
