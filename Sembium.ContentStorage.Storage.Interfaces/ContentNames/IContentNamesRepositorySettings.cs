using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.ContentNames
{
    public interface IContentNamesRepositorySettings
    {
        int MonthActiveVaultItemCount { get; }
        long MaxActiveVaultItemSize { get; }
        long MaxCompactVaultItemSize { get; }
    }
}
