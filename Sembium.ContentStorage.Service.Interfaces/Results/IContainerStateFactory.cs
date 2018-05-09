using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service.Results
{
    public delegate IContainerState IContainerStateFactory(string containerName, bool isReadOnly, bool isMaintained);
}
