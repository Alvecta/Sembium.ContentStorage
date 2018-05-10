using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Misc
{
    public class JavaScriptSerializer : ISerializer
    {
        private readonly IContractResolver _contractResolver;

        public JavaScriptSerializer(IContractResolver contractResolver)
        {
            _contractResolver = contractResolver;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings { ContractResolver = _contractResolver });
        }
    }
}
