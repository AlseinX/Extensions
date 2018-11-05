using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDisposableWithStatus : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsDisposed { get; }
    }
}