using System;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class DelegateRegistryBuilder : RegistryBuilderBase, IFactoryRegistryBuilder
    {
        private Delegate _factory;

        protected override Type ImplementationType => _factory.Method.ReturnType;

        protected override IRegistry BuildRegistry()
        => new DelegateRegistry(_lifetime, _injectProperties, _factory);

        public IFactoryRegistryBuilder UseFactory<TImplementation>(Func<IResolver, TImplementation> factory)
        {
            _factory = factory;
            return this;
        }
    }
}