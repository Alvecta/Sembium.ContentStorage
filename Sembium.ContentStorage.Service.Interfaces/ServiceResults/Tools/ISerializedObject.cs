using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public interface ISerializedObject
    {
        string DataTypeName { get; }
        string Data { get; }
    }
}
