using System;
using System.Threading.Tasks;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsyncDataSender : IDisposableWithStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SendAsync<TData>(TData data);
    }
}