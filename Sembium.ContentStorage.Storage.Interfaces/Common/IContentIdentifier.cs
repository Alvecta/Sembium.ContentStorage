using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Common
{
    public interface IContentIdentifier
    {
        string Hash { get; }
        string Extension { get; }
        string Guid { get; }
        DateTimeOffset ModifiedMoment { get; }
        bool Uncommitted { get; }
    }
}
