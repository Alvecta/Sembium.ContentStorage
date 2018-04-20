using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Common
{
    public class ContentNamesRepositorySettings
    {
        public int MonthActiveVaultItemCount { get; set; }
        public long MaxActiveVaultItemSize { get; set; }
        public long MaxCompactVaultItemSize { get; set; }
    }
}
