using System;
using System.Threading.Tasks;

namespace Alsein.Utilities
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
        Task<ITryResult<TData>> ReceiveAsync<TData>();

        /// <summary>
        /// 
        /// </summary>
        event Func<object, Task> Receive;
    }
}