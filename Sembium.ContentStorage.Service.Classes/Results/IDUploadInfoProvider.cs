using Sembium.ContentStorage.Service.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class IDUploadInfoProvider : IIDUploadInfoProvider
    {
        private readonly IIDUploadInfoFactory _idUploadInfoFactory;
        private readonly IUploadIdentifierSerializer _uploadIdentifierSerializer;

        public IDUploadInfoProvider(
            IIDUploadInfoFactory idUploadInfoFactory,
            IUploadIdentifierSerializer uploadIdentifierSerializer)
        {
            _idUploadInfoFactory = idUploadInfoFactory;
            _uploadIdentifierSerializer = uploadIdentifierSerializer;
        }

        public IIDUploadInfo GetIDUploadInfo(IUploadInfo uploadInfo)
        {
            if (uploadInfo == null)
                return null;

            return
                _idUploadInfoFactory(
                    uploadInfo.URL,
                    uploadInfo.HttpMethod,
                    uploadInfo.HttpHeaders,
                    _uploadIdentifierSerializer.Serialize(uploadInfo.UploadIdentifier)
                );
        }
    }
}
