using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.ServiceResults
{
    public interface IDocumentIdentifier
    {
        string Hash { get; }
        string Extension { get; }
    }
}
