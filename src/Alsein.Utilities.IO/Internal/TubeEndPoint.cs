using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO.Internal
{
    internal class TubeEndPoint : ITubeEndPoint
    {
        private ITubeOutlet _receiver;
        private ITubeInlet _sender;

        public TubeEndPoint(ITubeInlet sender, ITubeOutlet receiver)
        {
            _sender = sender;
            _receiver = receiver;
        }

        public bool IsDisposed => _receiver.IsDisposed;

        public event Func<TubeReceiveEventArgs, Task> Receive
        {
            add => _receiver.Receive += value;
            remove => _receiver.Receive -= value;
        }

        public void Dispose() => _receiver.Dispose();

        public Task<TData> ReceiveAsync<TData>() => _receiver.ReceiveAsync<TData>();

        public Task SendAsync<TData>(TData data) => _sender.SendAsync(data);
    }
}