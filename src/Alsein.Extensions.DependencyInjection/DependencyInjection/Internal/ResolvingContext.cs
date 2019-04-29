namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class ResolvingContext : IResolvingContext
    {
        public ResolvingContext(IScope scope) => Scope = scope;

        public IScope Scope { get; }
    }
}