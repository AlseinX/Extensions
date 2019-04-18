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
        /// <param name="sources"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
#if NETSTANDARD2_1
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, ValueTask<TResult>> selector)
            => sources.ToAsyncEnumerable().SelectAwait(selector);
#endif
#if NETSTANDARD2_0
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, ValueTask<TResult>> selector)
            => new Internal.AsyncSelector<TSource, TResult>(sources.ToAsyncEnumerable(), selector);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IAsyncEnumerable<TSource> sources, Func<TSource, ValueTask<TResult>> selector) =>
            new Internal.AsyncSelector<TSource, TResult>(sources, selector);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static async Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> sources)
        {
            var result = new List<TSource>();
            var enumerator = sources.GetEnumerator();
            while (await enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static async Task<TSource[]> ToArrayAsync<TSource>(this IAsyncEnumerable<TSource> sources) => (await sources.ToListAsync()).ToArray();
#endif
    }
}