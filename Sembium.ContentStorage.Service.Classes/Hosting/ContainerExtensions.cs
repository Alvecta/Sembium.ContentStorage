using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Service.Hosting
{
    public static class ContainerExtensions
    {
        public static string GetStringContent(this IContainer container, IContentIdentifier contentIdentifier)
        {
            if (!container.ContentExists(contentIdentifier))
                return null;

            var content = container.GetContent(contentIdentifier);

            using (var stream = content.GetReadStream())
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static void SetStringContent(this IContainer container, IContentIdentifier contentIdentifier, string stringContent)
        {
            var content = container.ContentExists(contentIdentifier) ? container.GetContent(contentIdentifier) : container.CreateContent(contentIdentifier);

            using (var stream = (string.IsNullOrEmpty(stringContent) ? new System.IO.MemoryStream() : new System.IO.MemoryStream(Encoding.UTF8.GetBytes(stringContent))))
            {
                content.LoadFromStream(stream);
            }
        }
    }
}
