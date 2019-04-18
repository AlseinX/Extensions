using System;

namespace Alsein.Extensions
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