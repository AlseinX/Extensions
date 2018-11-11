using System;
using System.Reflection;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDynamicInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="genericArgs"></param>
        /// <param name="valueArgs"></param>
        /// <returns></returns>
        object InvokeMethod(MethodInfo method, Type[] genericArgs, object[] valueArgs);
    }
}