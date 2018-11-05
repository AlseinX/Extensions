using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITubeInlet : IDisposableWithStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SendAsync<TData>(TData data);
    }
}