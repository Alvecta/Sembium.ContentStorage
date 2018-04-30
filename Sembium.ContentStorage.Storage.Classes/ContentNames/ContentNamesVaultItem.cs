using Microsoft.Extensions.Options;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.ContentNames
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

        public bool CanAppend(bool compacting)
        {
            return (_content.GetSize() < (compacting ? _contentNamesRepositorySettings.MaxCompactVaultItemSize : _contentNamesRepositorySettings.MaxActiveVaultItemSize));
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

        public void LoadFromStream(Stream stream)
        {
            _content.LoadFromStream(stream);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            await (_content as ISystemContent).DeleteAsync(cancellationToken);
        }

        public void Append(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
