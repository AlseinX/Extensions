using System;

namespace Alsein.Utilities.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult EvaluateOrDefault<TResult>(this Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default;
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
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TSource With<TSource>(this TSource source, Action<TSource> action)
        {
            action(source);
            return source;
        }
    }
}