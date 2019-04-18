namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class ModuleBuilder : IModuleBuilder
    {
        public IModule Build() => new Module();
    }
}