using System;
using System.Collections.Generic;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class Scope : IScope
    {
        private readonly object locker;

        private bool _isDisposed = false;

        private readonly IScope _parent;

        private readonly List<IModule> _modules;

        public Scope(IScope parent)
        {
            locker = new object();
            _parent = parent;
            _modules = new List<IModule>();
        }

        public event Action Dispose;

        public IEnumerable<IModule> Modules => _modules.AsReadOnly();

        public bool TryResolve(object key, out object result) => TryResolve(this, key, out result);

        private bool TryResolve(IResolver resolver, object key, out object result)
        {
            lock (locker)
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(nameof(IScope));
                }

                switch (key)
                {
                    case null:
                        throw new ArgumentNullException();

                    case Type type when type == typeof(IScope):
                    case var _ when key == this:
                        result = this;
                        return true;

                    case IScope scope:
                        result = new ScopedResolver(this, scope);
                        return true;

                    case Type type when type == typeof(IScopeFactory):
                        result = new ScopeFactory(this);
                        return true;
                }

                foreach (var module in _modules)
                {
                    if (module.TryResolve(resolver, key, out result))
                    {
                        return true;
                    }
                }

                if (_parent != null
                    && _parent.TryResolve<IResolver>(resolver, out var useResolver)
                    && useResolver.TryResolve(key, out result))
                {
                    return true;
                }

                result = default;
                return false;
            }
        }

        public void AddModule(IModule module)
        {
            lock (locker)
            {
                _modules.Add(module);
            }
        }

        public bool RemoveModule(IModule module)
        {
            lock (locker)
            {
                return _modules.Remove(module);
            }
        }

        object IServiceProvider.GetService(Type serviceType)
        => TryResolve(serviceType, out var result) ? result : default;

        void IDisposable.Dispose()
        {
            lock (locker)
            {
                if (_isDisposed)
                {
                    _isDisposed = true;
                    return;
                }

                Dispose?.Invoke();
            }
        }

        private sealed class ScopedResolver : IResolver
        {
            private readonly Scope _this;

            private readonly IScope _target;

            public ScopedResolver(Scope @this, IScope target)
            {
                _this = @this;
                _target = target;
            }

            public bool TryResolve(object key, out object result)
            => _this.TryResolve(_target, key, out result);
        }

        private sealed class ScopeFactory : IScopeFactory
        {
            private Scope _this;

            public ScopeFactory(Scope @this) => _this = @this;

            public IScope CreateChildScope()
            {
                lock (_this.locker)
                {
                    var result = new Scope(_this);
                    _this.Dispose += ((IDisposable)result).Dispose;
                    return result;
                }
            }
        }
    }
}