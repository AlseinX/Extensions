using System;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class DelegateRegistry : RegistryBase
    {
        private Delegate _factory;

        public DelegateRegistry(ILifetime lifetime, bool injectProperties, Delegate factory)
        : base(lifetime, injectProperties)
        => _factory = factory;

        public override object CreateInstance(IResolver resolver)
        {
            var result = _factory.DynamicInvoke(resolver);
            return InjectProperties ? resolver.InjectProperties(result) : result;
        }
    }
}