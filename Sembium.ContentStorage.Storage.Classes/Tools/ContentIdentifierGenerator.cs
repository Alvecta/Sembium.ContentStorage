using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public class ContentIdentifierGenerator : IContentIdentifierGenerator
    {
        private const string SSystem = "system";

        private readonly IContentIdentifierFactory _contentIdentifierFactory;

        public ContentIdentifierGenerator(IContentIdentifierFactory contentIdentifierFactory)
        {
            _contentIdentifierFactory = contentIdentifierFactory;
        }

        public IContentIdentifier GenerateContentIdentifier(string hash, string extension)
        {
            return _contentIdentifierFactory(hash, extension, GenerateGuid(), DateTimeOffset.UtcNow, true);
        }

        public IContentIdentifier GetCommittedContentIdentifier(IContentIdentifier contentIdentifier)
        {
            return _contentIdentifierFactory(contentIdentifier.Hash, contentIdentifier.Extension, contentIdentifier.Guid, contentIdentifier.ModifiedMoment, false);
        }

        public IContentIdentifier GetUncommittedContentIdentifier(IContentIdentifier contentIdentifier)
        {
            return _contentIdentifierFactory(contentIdentifier.Hash, contentIdentifier.Extension, contentIdentifier.Guid, contentIdentifier.ModifiedMoment, true);
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private string RemoveExtensionDot(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return extension;

            return extension.TrimStart('.');
        }

        public IContentIdentifier GetSystemContentIdentifier(string name)
        {
            return _contentIdentifierFactory(
                System.IO.Path.GetFileNameWithoutExtension(name),
                RemoveExtensionDot(System.IO.Path.GetExtension(name)),  
                SSystem,
                DateTimeOffset.Now,
                false);
        }

        public bool IsSystemContent(IContentIdentifier contentIdentifier)
        {
            return contentIdentifier.Hash.Equals(SSystem);
        }
    }
}
