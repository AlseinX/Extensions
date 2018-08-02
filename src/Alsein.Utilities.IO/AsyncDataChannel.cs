using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    internal class AsyncDataChannel : IDisposableWithStatus
    {

        private ConcurrentQueue<object> _stock;

        private ConcurrentQueue<TaskCompletionSource<TryResult<object>>> _quotes;

        public bool IsDisposed { get; private set; } = false;

        private AsyncDataChannel()
        {
            _stock = new ConcurrentQueue<object>();
            _quotes = new ConcurrentQueue<TaskCompletionSource<TryResult<object>>>();
        }

        private event Func<ReceiveEventArgs, Task> Receive;

        private class Sender : IAsyncDataSender
        {
            private AsyncDataChannel _channel;

            public Sender(AsyncDataChannel channel) => _channel = channel;

            public bool IsDisposed => _channel.IsDisposed;

            public void Dispose() => _channel.Dispose();

            public async Task SendAsync<TData>(TData data)
            {
                if (IsDisposed)
                {
                    throw new ChannelDisposedException();
                }
                var args = new ReceiveEventArgs(data);
                var invocations = _channel.Receive.GetInvocationList();
                if (invocations.Any())
                {
                    foreach (Func<ReceiveEventArgs, Task> invocation in invocations)
                    {
                        await invocation(args);
                        if (args.IsMonopolied)
                        {
                            break;
                        }
                    }
                    if (!args.IsAsyncReceived)
                    {
                        return;
                    }
                }
                var (hasQuote, value) = (false, default(TaskCompletionSource<TryResult<object>>));
                lock (_channel)
                {
                    if (IsDisposed)
                    {
                        throw new ChannelDisposedException();
                    }
                    hasQuote = _channel._quotes.TryDequeue(out value);
                }
                if (hasQuote)
                {
                    value.SetResult(new TryResult<object>(true, data));
                }
                else
                {
                    lock (_channel)
                    {
                        if (IsDisposed)
                        {
                            throw new ChannelDisposedException();
                        }
                        _channel._stock.Enqueue(data);
                    }
                }
            }
        }

        private class Receiver : IAsyncDataReceiver
        {
            private AsyncDataChannel _channel;

            public Receiver(AsyncDataChannel channel) => _channel = channel;

            public bool IsDisposed => _channel.IsDisposed;

            public event Func<ReceiveEventArgs, Task> Receive
            {
                add => _channel.Receive += value;
                remove => _channel.Receive -= value;
            }

            public void Dispose() => _channel.Dispose();

            public async Task<TData> ReceiveAsync<TData>()
            {
                var (hasStock, value) = (false, default(object));
                lock (_channel)
                {
                    if (IsDisposed)
                    {
                        throw new ChannelDisposedException();
                    }
                    hasStock = _channel._stock.TryDequeue(out value);
                }
                if (hasStock)
                {
                    return (TData)value;
                }
                else
                {
                    var completion = new TaskCompletionSource<TryResult<object>>();
                    lock (_channel)
                    {
                        if (_channel.IsDisposed)
                        {
                            throw new ChannelDisposedException();
                        }
                        _channel._quotes.Enqueue(completion);
                    }
                    var result = await completion.Task;
                    return (TData)result.Result;
                }
            }
        }

        public static (IAsyncDataSender, IAsyncDataReceiver) Create()
        {
            var channel = new AsyncDataChannel();
            return (new Sender(channel), new Receiver(channel));
        }

        public void Dispose() => _quotes.ForAll(x => x.SetResult(new TryResult<object>(false, null)));
    }
}