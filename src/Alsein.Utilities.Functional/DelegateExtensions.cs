using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult As<TResult>(this Delegate source) where TResult : Delegate
        {
            try
            {
                return source is TResult result ?
                    result :
                    (TResult)source.Method.CreateDelegate(typeof(TResult), source.Target);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidCastException(ex.Message, ex);
            }
        }
    }
}
