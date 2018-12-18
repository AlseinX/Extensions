using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alsein.Utilities
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
        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, Task<TResult>> selector) =>
            new RuntimeInternal.AsyncSelector<TSource, TResult>(sources.ToAsyncEnumerable(), selector);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> sources, Func<TSource, Task<TResult>> selector) =>
            new RuntimeInternal.AsyncSelector<TSource, TResult>(sources, selector);

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

        /// <summary>
        /// Executes <paramref name="action"/> for each element in <paramref name="source"/> and return the original <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="action">A transform function to apply to each element.</param>
        /// <param name="massiveExecutionFlags"></param>
        /// <returns>The original <paramref name="source"/>.</returns>
        public static async Task<IAsyncEnumerable<TSource>> ForAllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> action, MassiveExecutionFlags massiveExecutionFlags = MassiveExecutionFlags.ThrowWhenExceptionOccurs)
        {
            var exceptions = new List<Exception>();
            var enumerator = source.GetEnumerator();
            while (await enumerator.MoveNext())
            {
                try
                {
                    await action(enumerator.Current);
                }
                catch (Exception ex) when (massiveExecutionFlags != MassiveExecutionFlags.ThrowWhenExceptionOccurs)
                {
                    exceptions.Add(ex);
                }
            }
            if (massiveExecutionFlags == MassiveExecutionFlags.ThrowAggregateExceptions && exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
            return source;
        }

        /// <summary>
        /// Executes <paramref name="action"/> for each element in <paramref name="source"/> and return the original <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="action">A transform function to apply to each element.</param>
        /// <param name="massiveExecutionFlags"></param>
        /// <returns>The original <paramref name="source"/>.</returns>
        public static async Task<IEnumerable<TSource>> ForAllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> action, MassiveExecutionFlags massiveExecutionFlags = MassiveExecutionFlags.ThrowWhenExceptionOccurs)
        {
            await source.ToAsyncEnumerable().ForAllAsync(action);
            return source;
        }
    }
}