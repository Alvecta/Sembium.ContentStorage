using Sembium.ContentStorage.Service.ServiceResults.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class DocumentIDUploadInfoProvider : IDocumentIDUploadInfoProvider
    {
        private readonly IDocumentIDUploadInfoFactory _documentIDUploadInfoFactory;
        private readonly IIDUploadInfoProvider _idUploadInfoProvider;
        private readonly IDocumentIdentifierSerializer _documentIdentifierSerializer;

        public DocumentIDUploadInfoProvider(
            IDocumentIDUploadInfoFactory documentIDUploadInfoFactory,
            IIDUploadInfoProvider idUploadInfoProvider,
            IDocumentIdentifierSerializer documentIdentifierSerializer)
        {
            _documentIDUploadInfoFactory = documentIDUploadInfoFactory;
            _idUploadInfoProvider = idUploadInfoProvider;
            _documentIdentifierSerializer = documentIdentifierSerializer;
        }

        public IDocumentIDUploadInfo GetDocumentIDUploadInfo(IDocumentUploadInfo documentUploadInfo)
        {
            return
                _documentIDUploadInfoFactory(
                    _idUploadInfoProvider.GetIDUploadInfo(documentUploadInfo.UploadInfo),
                    _documentIdentifierSerializer.Serialize(documentUploadInfo.DocumentIdentifier)
                );
        }
    }
}
