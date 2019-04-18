using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DisposibleLocker : IDisposableWithStatus
    {
        private object _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public DisposibleLocker(object target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            Monitor.Enter(target);
            IsDisposed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked => Monitor.IsEntered(_target);

        /// <summary>
        /// 
        /// </summary>
        public event Action<DisposibleLocker> Disposing;

        /// <summary>
        /// 
        /// </summary>
        public event Action<DisposibleLocker> Disposed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
            Disposing?.Invoke(this);
            Monitor.Exit(_target);
            Disposed?.Invoke(this);
        }
    }
}
