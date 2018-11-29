using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common.ContentNames.Vault
{
    public class ContentNamesVaultItem : IContentNamesVaultItem
    {
        private readonly IContent _content;
        private readonly IContentNamesRepositorySettings _contentNamesRepositorySettings;

        public string Name { get; }
        public bool IsNew { get { return _isNew;  } }

        private readonly bool _isNew;

        public ContentNamesVaultItem(IContent content, string name, bool isNew,
            IContentNamesRepositorySettings contentNamesRepositorySettings)
        {
            _content = content;
            Name = name;
            _isNew = isNew;

            _contentNamesRepositorySettings = contentNamesRepositorySettings;
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
                return _content.GetContents(true);
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
