using System;
using System.Linq;
using System.Reflection;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class ReflectionConstructorRegistryBuilder : RegistryBuilderBase, IConstructorRegistryBuilder
    {
        private ConstructorInfo _constructor;

        private object[] _providedArgs;

        protected override Type ImplementationType => _constructor.DeclaringType;

        protected override IRegistry BuildRegistry()
        => new ReflectionConstructorRegistry(_lifetime, _injectProperties, _constructor, _providedArgs);

        IConstructorRegistryBuilder IConstructorRegistryBuilder.UseConstructor(ConstructorInfo constructor)
        {
            _constructor = constructor;
            return this;
        }

        IConstructorRegistryBuilder IConstructorRegistryBuilder.UseProvidedArgs(params object[] providedArgs)
        {
            _providedArgs = providedArgs;
            return this;
        }
    }
}