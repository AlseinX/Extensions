using System;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDynamicInvocable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericArgs"></param>
        /// <param name="valueArgs"></param>
        /// <returns></returns>
        object DispatchInvocation(int methodId, Type[] genericArgs, object[] valueArgs);
    }
}