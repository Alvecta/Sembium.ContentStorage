using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Tools
{
    public class ContentNameProvider : IContentNameProvider
    {
        private const char ContentNameSeparator = '-';
        private const string DateTimeFormat = "yyyyMMddHHmmss";
        private const string UncommittedString = "uncommitted";
        private const string SystemString = "system";

        private readonly IContentIdentifierFactory _contentIdentifierFactory;

        public ContentNameProvider(IContentIdentifierFactory contentIdentifierFactory)
        {
            _contentIdentifierFactory = contentIdentifierFactory;
        }

        private string GetContentName(IEnumerable<string> contentNameParts, string extension)
        {
            var contentName = string.Join(ContentNameSeparator.ToString(), contentNameParts);

            if (!string.IsNullOrEmpty(extension))
            {
                contentName = System.IO.Path.ChangeExtension(contentName, extension);
            }

            return contentName.ToLowerInvariant();
        }

        public string GetContentName(IContentIdentifier contentIdentifier)
        {
            if (contentIdentifier == null)
                return null;

            IEnumerable<string> contentNameParts = new[] 
                { 
                    contentIdentifier.Hash, 
                    contentIdentifier.ModifiedMoment.ToUniversalTime().ToString(DateTimeFormat), 
                    contentIdentifier.Guid
                };

            if (contentIdentifier.Uncommitted)
            {
                contentNameParts = contentNameParts.Concat(new[] { UncommittedString });
            }

            return GetSystemContentName(contentIdentifier) ?? GetContentName(contentNameParts, contentIdentifier.Extension);
        }

        private string GetSystemContentName(IContentIdentifier contentIdentifier)
        {
            if (contentIdentifier.Guid.Equals(SystemString))
                return System.IO.Path.ChangeExtension(contentIdentifier.Hash + ContentNameSeparator + SystemString, contentIdentifier.Extension);

            return null;
        }

        public IContentIdentifier GetContentIdentifier(string contentName) 
        {
            if (string.IsNullOrEmpty(contentName))
                return null;

            var extension = System.IO.Path.GetExtension(contentName);

            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.TrimStart('.').ToLowerInvariant();
            }

            var contentNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(contentName);

            var systemSuffix = "-" + SystemString;
            if (contentNameWithoutExtension.EndsWith(systemSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                return _contentIdentifierFactory(
                    contentNameWithoutExtension.Substring(0, contentNameWithoutExtension.Length - systemSuffix.Length),
                    extension, SystemString, DateTimeOffset.MinValue, false);
            }

            var parts = contentNameWithoutExtension.Split(ContentNameSeparator);
                
            if ((parts.Count() < 3) || (parts.Count() > 4))
                return null;

            if ((parts.Count() == 4) && (!parts[3].Equals(UncommittedString, StringComparison.InvariantCultureIgnoreCase)))
                return null;

            var hash = parts[0].ToLowerInvariant();
            var modifiedMoment = DateTimeOffset.ParseExact(parts[1], DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var guid = parts[2].ToLowerInvariant();
            var uncommitted = (parts.Count() == 4) && parts[3].Equals(UncommittedString, StringComparison.InvariantCultureIgnoreCase);

            return _contentIdentifierFactory(hash, extension, guid, modifiedMoment, uncommitted);
        }

        public string GetSearchPrefix(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return null;

            return hash + ContentNameSeparator;
        }
    }
}
