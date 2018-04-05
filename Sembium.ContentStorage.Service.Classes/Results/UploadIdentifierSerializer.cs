using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class UploadIdentifierSerializer : IUploadIdentifierSerializer
    {
        private readonly ISerializer _serializer;

        public UploadIdentifierSerializer(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string Serialize(IUploadIdentifier uploadIdentifier)
        {
            return _serializer.Serialize(uploadIdentifier);
        }

        public IUploadIdentifier Deserialize(string value)
        {
            return _serializer.Deserialize<UploadIdentifier>(value);
        }
    }
}
