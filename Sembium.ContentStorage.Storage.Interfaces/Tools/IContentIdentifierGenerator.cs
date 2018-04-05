using Sembium.ContentStorage.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public interface IContentIdentifierGenerator  
    {
        IContentIdentifier GenerateContentIdentifier(string hash, string extension);
        IContentIdentifier GetCommittedContentIdentifier(IContentIdentifier contentIdentifier);
        IContentIdentifier GetUncommittedContentIdentifier(IContentIdentifier contentIdentifier);
        IContentIdentifier GetSystemContentIdentifier(string name);
        bool IsSystemContent(IContentIdentifier contentIdentifier);
    }
}
