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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> creator)
        {
            lock (source)
            {
                if (!source.TryGetValue(key, out var result))
                {
                    source.Add(key, result = creator());
                }
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TValue>(this ICollection<TValue> source, Func<TValue, bool> predicate, Func<TValue> creator)
        {
            lock (source)
            {
                var result = source.FirstOrDefault(predicate);
                if (result == default)
                {
                    source.Add(result = creator());
                }
                return result;
            }
        }
    }
}