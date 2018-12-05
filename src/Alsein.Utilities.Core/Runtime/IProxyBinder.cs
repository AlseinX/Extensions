using System;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyBinder
    {
        /// <summary>
        /// 
        /// </summary>
        Type _target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object GetProxy(IDynamicInvoker invoker);
    }
}