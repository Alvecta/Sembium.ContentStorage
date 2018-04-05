using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults.Tools
{
    public class SerializedObject : ISerializedObject
    {
        public string DataTypeName { get; set; }
        public string Data { get; set; }

        public SerializedObject(string dataTypeName, string data)
        {
            DataTypeName = dataTypeName;
            Data = data;
        }
    }
}
