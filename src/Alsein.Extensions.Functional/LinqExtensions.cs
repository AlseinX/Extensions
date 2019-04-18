using Alsein.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Extensions
{
    /// <summary>
    /// Provides Linq-like functional programming extension methods.
    /// </summary>
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="exceptionFactory"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Assert<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, Exception> exceptionFactory) =>
            source.Select(item => predicate(item) ? item : throw exceptionFactory(item));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Cross<TSource, TDestination, TResult>(this IEnumerable<TSource> source, IEnumerable<TDestination> destination, Func<TSource, TDestination, TResult> selector)
        {
            foreach (var item1 in source)
            {
                foreach (var item2 in destination)
                {
                    yield return selector(item1, item2);
                }
            }
        }

        /// <summary>
        /// Executes <paramref name="action"/> for each element in <paramref name="source"/> and return the original <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="action">A transform function to apply to each element.</param>
        /// <param name="massiveExecutionFlags"></param>
        /// <returns>The original <paramref name="source"/>.</returns>
        public static IEnumerable<TSource> ForAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action, MassiveExecutionFlags massiveExecutionFlags = MassiveExecutionFlags.ThrowWhenExceptionOccurs)
        {
            var exceptions = new List<Exception>();
            foreach (var item in source)
            {
                try
                {
                    action(item);
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
        /// Executes <paramref name="func"/> for each element in <paramref name="source"/> and return the results set.
        /// <remarks>This operation is executed immidiately instead of deferred execution.</remarks>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="func"/>.</typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="massiveExecutionFlags"></param>
        /// <returns>An System.Collections.Generic.IEnumerable&lt;<typeparamref name="TSource"/>&gt; whose elements are the result of invoking <paramref name="func"/> on each element of <paramref name="source"/>.</returns>
        public static IEnumerable<TResult> ForAll<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func, MassiveExecutionFlags massiveExecutionFlags = MassiveExecutionFlags.ThrowWhenExceptionOccurs)
        {
            var results = new List<TResult>();
            var exceptions = new List<Exception>();
            foreach (var item in source)
            {
                try
                {
                    results.Add(func(item));
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
            return results.AsEnumerable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, TSource>> Indexed<TSource>(this IEnumerable<TSource> source) => source.Select((item, index) => new KeyValuePair<int, TSource>(index, item));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item) => source.IndexOf(i => EqualityComparer<TSource>.Default.Equals(i, item));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var i = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<TResult>(this IEnumerable<TResult> source, string separator = "") => string.Join(separator, source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TSource> LockedCache<TSource>(this IEnumerable<TSource> source)
        {
            lock (source)
            {
                return source.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Mess<TSource>(this IEnumerable<TSource> source, Random random = null)
        {
            var arr = source.ToArray();
            var pickeds = false.Plural(arr.Length).ToArray();
            var num = arr.Length;
            random = random ?? new Random();
            while (num > 0)
            {
                var r = random.Next(num);
                var index = r;
                for (var i = 0; i <= index; i++)
                {
                    while (pickeds[i])
                    {
                        index++;
                        i++;
                    }
                }
                pickeds[index] = true;
                yield return arr[index];
                num--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="recurser"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TSource> PosteritiesOf<TSource>(this IEnumerable<TSource> source, TSource target, Func<TSource, TSource> recurser)
        {
            source = source.ToArray();
            var found = new List<TSource>
        {
            target
        };
        start:
            foreach (var item in source)
            {
                if (found.Contains(item))
                {
                    continue;
                }
                if (found.Any(parent => recurser(item)?.Equals(parent) ?? false))
                {
                    found.Add(item);
                    yield return item;
                    goto start;
                }
            }
            yield break;
        }

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
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WithAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action) => source.Select(item => item.With(action));
    }
}