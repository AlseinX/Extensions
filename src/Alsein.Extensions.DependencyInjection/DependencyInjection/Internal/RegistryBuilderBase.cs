using System;
using System.Collections.Generic;
using Alsein.Extensions.Patterns;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal abstract class RegistryBuilderBase : IRegistryBuilder
    {
        protected ILifetime _lifetime;

        protected bool _injectProperties = true;

        protected abstract Type ImplementationType { get; }

        private IList<object> _keys = new List<object>();

        public IRegistryBuilder InjectProperties(bool enabled = true)
        {
            _injectProperties = enabled;
            return this;
        }

        public IRegistryBuilder Singleton()
        {
            _lifetime = null;
            return this;
        }

        protected abstract IRegistry BuildRegistry();

        public (IEnumerable<object>, IRegistry) Build()
        {
            if (_keys.Count == 0)
            {
                _keys.Add(ImplementationType);
            }
            return (_keys, BuildRegistry());
        }

        IRegistryBuilder IRegistryBuilder.InjectProperties(bool enabled)
        {
            _injectProperties = enabled;
            return this;
        }

        IRegistryBuilder IRegistryBuilder.To(object key)
        {
            _keys.Add(key);
            return this;
        }
    }
}