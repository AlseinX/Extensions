using System;
using System.Threading.Tasks;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplexAsyncDataEndPoint : IAsyncDataEndPoint
    {
        private IAsyncDataReceiver _receiver;
        private IAsyncDataSender _sender;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        public DuplexAsyncDataEndPoint(IAsyncDataSender sender, IAsyncDataReceiver receiver)
        {
            _sender = sender;
            _receiver = receiver;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed => _receiver.IsDisposed;

        /// <summary>
        /// 
        /// </summary>
        public event Func<object, Task> Receive
        {
            add => _receiver.Receive += value;
            remove => _receiver.Receive -= value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => _receiver.Dispose();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ITryResult<TData>> ReceiveAsync<TData>() => _receiver.ReceiveAsync<TData>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task SendAsync<TData>(TData data) => _sender.SendAsync(data);
    }
}