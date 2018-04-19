using Microsoft.Extensions.Options;
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
        private readonly ContentNamesRepositorySettings _contentNamesRepositorySettings;

        public string Name { get; }

        private readonly bool _isNew;

        public ContentNamesVaultItem(IContent content, string name, bool isNew,
            IOptions<ContentNamesRepositorySettings> contentNamesRepositorySettings)
        {
            _content = content;
            Name = name;
            _isNew = isNew;

            _contentNamesRepositorySettings = contentNamesRepositorySettings.Value;
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
            return (_content.GetSize() < _contentNamesRepositorySettings.MaxMonthVaultItemSize);
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
