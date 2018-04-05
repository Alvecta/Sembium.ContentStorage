using Sembium.ContentStorage.Service.ServiceResults.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public class DocumentMultiPartIDUploadInfoProvider : IDocumentMultiPartIDUploadInfoProvider
    {
        private readonly IDocumentMultiPartIDUploadInfoFactory _documentMultiPartIDUploadInfoFactory;
        private readonly IMultiPartIDUploadInfoProvider _MultiPartIDUploadInfoProvider;
        private readonly IDocumentIdentifierSerializer _documentIdentifierSerializer;

        public DocumentMultiPartIDUploadInfoProvider(
            IDocumentMultiPartIDUploadInfoFactory documentMultiPartIDUploadInfoFactory,
            IMultiPartIDUploadInfoProvider MultiPartIDUploadInfoProvider,
            IDocumentIdentifierSerializer documentIdentifierSerializer)
        {
            _documentMultiPartIDUploadInfoFactory = documentMultiPartIDUploadInfoFactory;
            _MultiPartIDUploadInfoProvider = MultiPartIDUploadInfoProvider;
            _documentIdentifierSerializer = documentIdentifierSerializer;
        }

        public IDocumentMultiPartIDUploadInfo GetDocumentMultiPartIDUploadInfo(IDocumentMultiPartUploadInfo documentMultiPartUploadInfo)
        {
            return
                _documentMultiPartIDUploadInfoFactory(
                    _MultiPartIDUploadInfoProvider.GetMultiPartIDUploadInfo(documentMultiPartUploadInfo.MultiPartUploadInfo),
                    _documentIdentifierSerializer.Serialize(documentMultiPartUploadInfo.DocumentIdentifier)
                );
        }
    }
}
