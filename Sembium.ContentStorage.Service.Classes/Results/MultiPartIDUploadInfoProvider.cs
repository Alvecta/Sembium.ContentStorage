using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Service.ServiceResults.Factories;
using Sembium.ContentStorage.Storage.HostingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class MultiPartIDUploadInfoProvider : IMultiPartIDUploadInfoProvider
    {
        private readonly IMultiPartIDUploadInfoFactory _MultiPartIDUploadInfoFactory;
        private readonly IUploadIdentifierSerializer _uploadIdentifierSerializer;

        public MultiPartIDUploadInfoProvider(
            IMultiPartIDUploadInfoFactory MultiPartIDUploadInfoFactory,
            IUploadIdentifierSerializer uploadIdentifierSerializer)
        {
            _MultiPartIDUploadInfoFactory = MultiPartIDUploadInfoFactory;
            _uploadIdentifierSerializer = uploadIdentifierSerializer;
        }

        public IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(IMultiPartUploadInfo multiPartUploadInfo)
        {
            if (multiPartUploadInfo == null)
                return null;

            return
                _MultiPartIDUploadInfoFactory(
                    multiPartUploadInfo.HttpMethod,
                    multiPartUploadInfo.PartSize,
                    multiPartUploadInfo.PartUploadInfos,
                    multiPartUploadInfo.MultiPartUploadResultHeaderName,
                    _uploadIdentifierSerializer.Serialize(multiPartUploadInfo.UploadIdentifier)
                );
        }
    }
}
