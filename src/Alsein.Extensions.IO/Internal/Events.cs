using Alsein.Extensions.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Extensions.IO.Internal
{
    internal class Events<TTarget> : IEvents<TTarget>, IChainDisposable
    where TTarget : class
    {
        public Events(EventPool pool, TTarget target)
        {
            _isDisposed = false;
            _passiveHandlers = new List<Delegate>();
            _activeHandlers = new List<Quote>();
            _pool = pool;
            Target = target;
        }

        private bool _isDisposed;

        private List<Delegate> _passiveHandlers;

        private List<Quote> _activeHandlers;

        private EventPool _pool;

        private Func<object> _parentSelector;

        public TTarget Target { get; }

        public IEvents<object> Parent => _pool.GetEvents(_parentSelector?.Invoke());

        public IEvents<TTarget> SetParent<TParent>(Func<TParent> selector) where TParent : class
        {
            _parentSelector = selector;
            return this;
        }

        public void Dispose()
        {
            DisposeSelf();
            _pool.Values.PosteritiesOf(this, x => x.Parent).LockedCache().ForAll(x => (x as IChainDisposable)?.DisposeSelf());

        }

        public void DisposeSelf()
        {
            _isDisposed = true;
            Flush();
            _pool.Reset(Target);
        }

        private void NoDisposed()
        {
            if (_isDisposed)
            {
                throw new InvalidOperationException();
            }
        }

        public IEvents<TTarget> FireEvent<TEvent>(TEvent eventObject, EventDiffusionOptions options)
        where TEvent : class
        {
            NoDisposed();
            switch (options)
            {
                case EventDiffusionOptions.None:
                    FireEvent(eventObject);
                    break;

                case EventDiffusionOptions.Popup:
                    this.Recurse<IEvents<object>>(x => x.Parent).LockedCache().ForAll(x => x.FireEvent(eventObject));
                    break;

                case EventDiffusionOptions.RecurseDown:
                    FireEvent(eventObject);
                    _pool.Values.PosteritiesOf(this, x => x.Parent).LockedCache().ForAll(x => x.FireEvent(eventObject));
                    break;
            }
            return this;
        }

        private IEnumerable<Quote> TakeActiveHandlers()
        {
            var matches = default(IEnumerable<Quote>);
            lock (_activeHandlers)
            {
                matches = _activeHandlers
                    .ToArray();
                _activeHandlers.Clear();
            }
            return matches;
        }

        private IEnumerable<Quote> TakeActiveHandlers<TEvent>()
        {
            var matches = default(IEnumerable<Quote>);
            lock (_activeHandlers)
            {
                matches = _activeHandlers
                    .Where(quote => quote.Type.IsAssignableFrom(typeof(TEvent)))
                    .ToArray();
                matches.ForAll(_activeHandlers.Remove);
            }
            return matches;
        }

        private void FireEvent<TEvent>(TEvent eventObject)
        where TEvent : class
        {
            var context = new EventContext<TEvent>(eventObject, this);

            foreach (Action<IEventContext<TEvent>> action in _passiveHandlers.LockedCache())
            {
                action(context);
            }

            foreach (var quote in TakeActiveHandlers<TEvent>())
            {
                quote.Source.SetResult(context);
            }
        }

        public IEvents<TTarget> AddEventHandler<TEvent>(Action<IEventContext<TEvent>> action)
        where TEvent : class
        {
            NoDisposed();
            lock (_passiveHandlers)
            {
                _passiveHandlers.Add(action);
            }
            return this;
        }

        public async Task<IEventContext<TEvent>> NextEventAsync<TEvent>()
        where TEvent : class
        {
            NoDisposed();
            var source = new TaskCompletionSource<IEventContext<object>>();
            lock (_activeHandlers)
            {
                _activeHandlers.Add(new Quote(source, typeof(TEvent)));
            }
            return (IEventContext<TEvent>)await source.Task;
        }

        public IEvents<TTarget> RemoveEventHandler<TEvent>(Action<IEventContext<TEvent>> action) where TEvent : class
        {
            NoDisposed();
            lock (_passiveHandlers)
            {
                _passiveHandlers.Remove(action);
            }
            return this;
        }

        public bool HasEventHandler<TEvent>(Action<IEventContext<TEvent>> action) where TEvent : class
        {
            NoDisposed();
            lock (_passiveHandlers)
            {
                return _passiveHandlers.Contains(action);
            }
        }

        public IEvents<TTarget> Flush()
        {
            foreach (var quote in TakeActiveHandlers())
            {
                quote.Source.SetCanceled();
            }
            return this;
        }

        public IEvents<TTarget> Flush<TEvent>() where TEvent : class
        {
            foreach (var quote in TakeActiveHandlers<TEvent>())
            {
                quote.Source.SetCanceled();
            }
            return this;
        }
    }
}