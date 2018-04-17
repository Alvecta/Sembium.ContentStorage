using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.Hosting
{
    public interface ISystemContainer : IContainer
    {
        IContent GetContent(string contentName);
        IEnumerable<IContent> GetContents(string prefix);
    }
}
