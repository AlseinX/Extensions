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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        object GetGlobalVariable(string variableName, Type valueType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        void SetGlobalVariable(string variableName, object value);

        /// <summary>
        /// 
        /// </summary>
        event Action<INativeModule, EventArgs> Disposing;

        /// <summary>
        /// 
        /// </summary>
        event Action<INativeModule, EventArgs> Disposed;
    }
}
