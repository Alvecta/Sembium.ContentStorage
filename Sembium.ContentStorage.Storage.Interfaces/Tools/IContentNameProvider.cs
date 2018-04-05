using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public interface IContentNameProvider
    {
        string GetContentName(IContentIdentifier contentIdentifier);
        IContentIdentifier GetContentIdentifier(string contentName);
        string GetSearchPrefix(string hash);
    }
}
