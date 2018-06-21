using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Utilities
{
    /// <summary>
    /// Provides Linq-like functional programming extension methods.
    /// </summary>
    public static partial class FunctionalExtensions
    {
        /// <summary>
        /// Executes <paramref name="action"/> for each element in <paramref name="source"/> and return the original <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="action">A transform function to apply to each element.</param>
        /// <returns>The original <paramref name="source"/>.</returns>
        public static IEnumerable<TSource> ForAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
            return source;
        }

        /// <summary>
        /// Executes <paramref name="func"/> for each element in <paramref name="source"/> and return the results set.
        /// <remarks>This operation is executed immidiately instead of deferred execution.</remarks>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="func"/>.</typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns>An System.Collections.Generic.IEnumerable&lt;<typeparamref name="TSource"/>&gt; whose elements are the result of invoking <paramref name="func"/> on each element of <paramref name="source"/>.</returns>
        public static IEnumerable<TResult> ForAll<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func) => source.Select(func).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<TResult>(this IEnumerable<TResult> source, string separator) => string.Join(separator, source);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Plural<TResult>(this TResult source, int times = 1)
        {
            for (var i = 0; i < times; i++)
            {
                yield return source;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="recurser"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Recurse<TSource>(this TSource source, Func<TSource, TSource> recurser)
        {
            while (source != null)
            {
                yield return source;
                source = recurser(source);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="recurser"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Recurse<TSource, TResult>(this TSource source, Func<TSource, TResult> selector, Func<TResult, TSource> recurser)
        {
            while (source != null)
            {
                var result = selector(source);
                yield return result;
                source = recurser(result);
            }
        }

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
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult Return<TSource, TResult>(this IUsingQuotation<TSource> source, Func<TSource, TResult> func) => source.Using(func).Return();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<int> To(this int from, int to, int step = 1)
        {
            if (from == to)
            {
                yield return from;
                yield break;
            }
            var inc = from < to;
            step = Math.Abs(step) * (from < to ? 1 : -1);
            for (var i = from; i < to == inc || i == to; i += step)
            {
                yield return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static TSource To<TSource>(this TSource source, out TSource dest) => dest = source;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TSource To<TSource>(this TSource source, Action<TSource> action)
        {
            action(source);
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult To<TSource, TResult>(this TSource source, Func<TSource, TResult> func) => func(source);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IUsingQuotation<TResult> Using<TSource, TResult>(this TSource source, Func<TResult> func) => new TopUsingQuotation<TResult>(new Func<TSource, TResult>(x => func()), source);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IUsingQuotation<TResult> Using<TSource, TResult>(this TSource source, Func<TSource, TResult> func) => new TopUsingQuotation<TResult>(func, source);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TSource With<TSource>(this TSource source, Action<TSource> action)
        {
            action(source);
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WithAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action) => source.Select(item => item.With(action));
    }
}