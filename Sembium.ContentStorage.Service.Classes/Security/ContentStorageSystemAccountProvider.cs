using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageSystemAccountProvider : IContentStorageSystemAccountProvider
    {
        private readonly IContentStorageAccountFactory _contentStorageAccountFactory;
        private readonly IContentStorageSystemPrincipalProvider _contentStorageSystemPrincipalProvider;
        private readonly IContentStoragePrincipalManager _contentStoragePrincipalManager;

        public ContentStorageSystemAccountProvider(
            IContentStorageAccountFactory contentStorageAccountFactory,
            IContentStorageSystemPrincipalProvider contentStorageSystemPrincipalProvider,
            IContentStoragePrincipalManager contentStoragePrincipalManager)
        {
            _contentStorageAccountFactory = contentStorageAccountFactory;
            _contentStorageSystemPrincipalProvider = contentStorageSystemPrincipalProvider;
            _contentStoragePrincipalManager = contentStoragePrincipalManager;
        }

        public IContentStorageAccount GetAccount()
        {
            _contentStoragePrincipalManager.Principal = _contentStorageSystemPrincipalProvider.GetPrincipal();

            return _contentStorageAccountFactory();
        }
    }
}
