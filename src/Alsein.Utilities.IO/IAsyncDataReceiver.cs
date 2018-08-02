using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsyncDataReceiver : IDisposableWithStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TData> ReceiveAsync<TData>();

        /// <summary>
        /// 
        /// </summary>
        event Func<ReceiveEventArgs, Task> Receive;
    }
}