using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Common
{
    public delegate IContentIdentifier IContentIdentifierFactory(string hash, string extension, string guid, DateTimeOffset modifiedMoment, bool uncommitted);
}
