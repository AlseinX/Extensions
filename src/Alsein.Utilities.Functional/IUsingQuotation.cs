using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IUsingQuotation<out TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        IUsingQuotation<TNew> Using<TNew>(Func<TResult, TNew> func);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TResult Return();
    }
}