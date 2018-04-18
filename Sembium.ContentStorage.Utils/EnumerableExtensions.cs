using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sembium.ContentStorage.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> UniqueOnOrdered<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            var prevAssigned = false;
            var previous = default(TSource);

            foreach (var item in source)
            {
                if ((!prevAssigned) || (!comparer.Equals(previous, item)))
                {
                    yield return item;
                }

                previous = item;
                prevAssigned = true;
            }
        }

        public static IEnumerable<TSource> UniqueOnOrdered<TSource>(this IEnumerable<TSource> source)
        {
            return source.UniqueOnOrdered(EqualityComparer<TSource>.Default);
        }
    }
}
