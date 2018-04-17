using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sembium.ContentStorage.Common
{
    public class ContentNamesVaultItem : IContentNamesVaultItem
    {
        private readonly IContent _content;

        public string Name { get; }

        private readonly bool _isNew;

        public ContentNamesVaultItem(IContent content, string name, bool isNew)
        {
            _content = content;
            Name = name;
            _isNew = isNew;
        }

        public void Append(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                using (var rs = OpenReadStream())
                {
                    rs.CopyTo(ms);
                    stream.CopyTo(ms);
                }

                ms.Position = 0;
                _content.LoadFromStream(ms);
            }
        }

        public bool CanAppend()
        {
            return _content.GetSize() < 512 * 1024;  // todo: config
        }

        public Stream OpenReadStream()
        {
            if (_isNew)
            {
                return new MemoryStream();
            }
            else
            {
                return _content.GetReadStream();
            }
        }
    }
}
