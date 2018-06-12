using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
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
    }
}