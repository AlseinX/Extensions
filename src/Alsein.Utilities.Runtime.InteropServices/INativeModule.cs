using System;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface INativeModule : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="delegateType"></param>
        /// <returns></returns>
        Delegate GetFunction(string functionName, Type delegateType);
    }
}
