using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO.Internal
{
    internal class SimplexTube : IDisposableWithStatus
    {

        private ConcurrentQueue<object> _stock;

        private ConcurrentQueue<TaskCompletionSource<IFailableResult<object>>> _quotes;

        public bool IsDisposed { get; private set; } = false;

        private SimplexTube()
        {
            _stock = new ConcurrentQueue<object>();
            _quotes = new ConcurrentQueue<TaskCompletionSource<IFailableResult<object>>>();
        }

        private event Func<TubeReceiveEventArgs, Task> Receive;

        private class Sender : ITubeInlet
        {
            private SimplexTube _channel;

            public Sender(SimplexTube channel) => _channel = channel;

            public bool IsDisposed => _channel.IsDisposed;

            public void Dispose() => _channel.Dispose();

            public async Task SendAsync<TData>(TData data)
            {
                if (IsDisposed)
                {
                    throw new TubeDisposedException();
                }
                var args = new TubeReceiveEventArgs(data);
                var invocations = _channel.Receive?.GetInvocationList();
                if (invocations?.Any() ?? false)
                {
                    foreach (Func<TubeReceiveEventArgs, Task> invocation in invocations)
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
                var (hasQuote, value) = (false, default(TaskCompletionSource<IFailableResult<object>>));
                lock (_channel)
                {
                    if (IsDisposed)
                    {
                        throw new TubeDisposedException();
                    }
                    hasQuote = _channel._quotes.TryDequeue(out value);
                }
                if (hasQuote)
                {
                    value.SetResult(FailableResult.Create<object>(true, data));
                }
                else
                {
                    lock (_channel)
                    {
                        if (IsDisposed)
                        {
                            throw new TubeDisposedException();
                        }
                        _channel._stock.Enqueue(data);
                    }
                }
            }
        }

        private class Receiver : ITubeOutlet
        {
            private SimplexTube _channel;

            public Receiver(SimplexTube channel) => _channel = channel;

            public bool IsDisposed => _channel.IsDisposed;

            public event Func<TubeReceiveEventArgs, Task> Receive
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
                        throw new TubeDisposedException();
                    }
                    hasStock = _channel._stock.TryDequeue(out value);
                }
                if (hasStock)
                {
                    return (TData)value;
                }
                else
                {
                    var completion = new TaskCompletionSource<IFailableResult<object>>();
                    lock (_channel)
                    {
                        if (_channel.IsDisposed)
                        {
                            throw new TubeDisposedException();
                        }
                        _channel._quotes.Enqueue(completion);
                    }
                    var result = await completion.Task;
                    return (TData)result.Result;
                }
            }
        }

        public static (ITubeInlet, ITubeOutlet) Create()
        {
            var channel = new SimplexTube();
            return (new Sender(channel), new Receiver(channel));
        }

        public void Dispose() => _quotes.ForAll(x => x.SetResult(FailableResult.Create<object>(false, null)));
    }
}