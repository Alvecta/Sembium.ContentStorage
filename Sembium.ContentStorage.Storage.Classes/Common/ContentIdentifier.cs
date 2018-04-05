using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.Common
{
    public class ContentIdentifier : IContentIdentifier
    {
        public string Hash { get; private set; }
        public string Extension { get; private set; }
        public string Guid { get; private set; }
        public DateTimeOffset ModifiedMoment { get; private set; }
        public bool Uncommitted { get; private set; }

        public ContentIdentifier(string hash, string extension, string guid, DateTimeOffset modifiedMoment, bool uncommitted)
        {
            Hash = hash;
            Extension = extension;
            Guid = guid;
            ModifiedMoment = modifiedMoment;
            Uncommitted = uncommitted;
        }

        public override bool Equals(object obj)
        {
            var contentIdentifier = obj as ContentIdentifier;

            if (contentIdentifier == null)
                return false;

            return
                string.Equals(Hash, contentIdentifier.Hash) &&
                string.Equals(Extension, contentIdentifier.Extension) &&
                string.Equals(Guid, contentIdentifier.Guid) &&
                DateTimeOffset.Equals(ModifiedMoment, contentIdentifier.ModifiedMoment) &&
                bool.Equals(Uncommitted, contentIdentifier.Uncommitted);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return
                    (string.IsNullOrEmpty(Hash) ? 0 : Hash.GetHashCode()) +
                    (string.IsNullOrEmpty(Extension) ? 0 : Extension.GetHashCode()) +
                    (string.IsNullOrEmpty(Guid) ? 0 : Guid.GetHashCode()) +
                    ModifiedMoment.GetHashCode() +
                    Uncommitted.GetHashCode();
            }
        }
    }
}
