using System;
using System.Collections.Generic;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class ReflectionModuleBuilder : IModuleBuilder
    {
        public IDictionary<object, IRegistry> Registries { get; } = new Dictionary<object, IRegistry>();

        public IModule Build() => new ReflectionModule(new Dictionary<object, IRegistry>(Registries));

        private void Register(IRegistryBuilder builder)
        {
            var (types, registry) = builder.Build();
            foreach (var key in types)
            {
                Registries.Add(key, registry);
            }
        }

        public IModuleBuilder Register(Action<IConstructorRegistryBuilder> action)
        {
            var builder = new ReflectionConstructorRegistryBuilder();
            action?.Invoke(builder);
            Register(builder);
            return this;
        }

        public IModuleBuilder Register(Action<IFactoryRegistryBuilder> action)
        {
            var builder = new DelegateRegistryBuilder();
            action?.Invoke(builder);
            Register(builder);
            return this;
        }
    }
}