using System;
using System.Collections.Generic;

namespace Alsein.Utilities.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventPool : IReadOnlyDictionary<object, IEvents<object>>, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        IEvents<TTarget> GetEvents<TTarget>(TTarget target)
        where TTarget : class;

        /// <summary>
        /// 
        /// </summary>
        void Reset();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        void Reset(object target);
    }
}