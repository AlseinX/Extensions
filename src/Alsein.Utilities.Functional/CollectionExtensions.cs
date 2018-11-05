using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        public static void RemoveWhere<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate) => source.Where(predicate).ForAll(source.Remove);
    }
}