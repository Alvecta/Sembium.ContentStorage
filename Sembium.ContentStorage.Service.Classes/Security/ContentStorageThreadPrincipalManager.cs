using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Security
{
    public class ContentStorageThreadPrincipalManager : IContentStoragePrincipalManager
    {
        private IContentStoragePrincipal _principal;

        public IContentStoragePrincipal Principal
        {
            get
            {
                return _principal;
            }

            set
            {
                _principal = value;
                SetPrincipal(_principal);
            }
        }

        protected virtual void SetPrincipal(IContentStoragePrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
        }
    }
}
