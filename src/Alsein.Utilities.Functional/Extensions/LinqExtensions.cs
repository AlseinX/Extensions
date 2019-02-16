using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Utilities.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqExtensions
    {
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
    }
}
