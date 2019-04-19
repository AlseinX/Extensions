using System;
using System.Collections.Generic;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class ReflectionModule : IModule
    {
        private IDictionary<object, IRegistry> _registries;

        public ReflectionModule(IDictionary<object, IRegistry> registries)
        {
            _registries = registries;
        }

        public bool TryResolve(IResolver resolver, object key, out object result)
        {
            if (!_registries.TryGetValue(key, out var registry))
            {
                result = null;
                return false;
            }

            result = registry.CreateInstance(resolver);
            return true;
        }
    }
}