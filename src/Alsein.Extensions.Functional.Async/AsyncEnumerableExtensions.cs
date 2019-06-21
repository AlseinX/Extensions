using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
            => source.ToAsyncEnumerable().SelectAwait(selector);

#if NETSTANDARD2_0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static async IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static async IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            await foreach (var item in source)
            {
                yield return await selector(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static async Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            var result = new List<TSource>();
            await foreach (var item in source)
            {
                result.Add(item);
            }
            return result;
        }
#endif
    }
}