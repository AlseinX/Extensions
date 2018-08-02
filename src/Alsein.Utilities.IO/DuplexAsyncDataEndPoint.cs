using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    internal class DuplexAsyncDataEndPoint : IAsyncDataEndPoint
    {
        private IAsyncDataReceiver _receiver;
        private IAsyncDataSender _sender;

        public DuplexAsyncDataEndPoint(IAsyncDataSender sender, IAsyncDataReceiver receiver)
        {
            _sender = sender;
            _receiver = receiver;
        }

        public bool IsDisposed => _receiver.IsDisposed;

        public event Func<ReceiveEventArgs, Task> Receive
        {
            add => _receiver.Receive += value;
            remove => _receiver.Receive -= value;
        }

        public void Dispose() => _receiver.Dispose();

        public Task<TData> ReceiveAsync<TData>() => _receiver.ReceiveAsync<TData>();

        public Task SendAsync<TData>(TData data) => _sender.SendAsync(data);
    }
}