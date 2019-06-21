using Alsein.Extensions.DependencyInjection.Internal;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IModuleBuilder CreateBuilder() => new ReflectionModuleBuilder();
    }
}