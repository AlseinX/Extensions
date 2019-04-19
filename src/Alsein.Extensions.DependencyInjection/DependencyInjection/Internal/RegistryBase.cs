namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal abstract class RegistryBase : IRegistry
    {
        public ILifetime Lifetime { get; }

        public bool InjectProperties { get; }

        public RegistryBase(ILifetime lifetime, bool injectProperties)
        {
            Lifetime = lifetime;
            InjectProperties = injectProperties;
        }

        public abstract object CreateInstance(IResolver resolver);
    }
}