using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Utilities
{
    public static partial class FunctionalExtensions
    {
        public static IEnumerable<TSource> ForAll<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
            return source;
        }

        public static IEnumerable<TResult> ForAll<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func) => source.Select(func).ToArray();

        public static string Join<TResult>(this IEnumerable<TResult> source, string separator) => string.Join(separator, source);

        public static IEnumerable<TSource> Recurse<TSource>(this TSource source, Func<TSource, TSource> recurser)
        {
            while (source != null)
            {
                yield return source;
                source = recurser(source);
            }
        }

        public static IEnumerable<TResult> Recurse<TSource, TResult>(this TSource source, Func<TSource, TResult> selector, Func<TResult, TSource> recurser)
        {
            while (source != null)
            {
                var result = selector(source);
                yield return result;
                source = recurser(result);
            }
        }

        public static void RemoveWhere<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate) => source.Where(predicate).ForAll(source.Remove);

        public static TResult Return<TSource, TResult>(this IUsingQuotation<TSource> source, Func<TSource, TResult> func) => source.Using(func).Return();

        public static IEnumerable<int> To(this int from, int to) => Enumerable.Range(from, to - from);

        public static TSource To<TSource>(this TSource source, Action<TSource> action)
        {
            action(source);
            return source;
        }

        public static TResult To<TSource, TResult>(this TSource source, Func<TSource, TResult> func) => func(source);

        public static IUsingQuotation<TResult> Using<TSource, TResult>(this TSource source, Func<TResult> func) => new TopUsingQuotation<TResult>(new Func<TSource, TResult>(x => func()), source);

        public static IUsingQuotation<TResult> Using<TSource, TResult>(this TSource source, Func<TSource, TResult> func) => new TopUsingQuotation<TResult>(func, source);
    }
}