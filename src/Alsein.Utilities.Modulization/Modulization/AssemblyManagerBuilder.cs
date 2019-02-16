using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyManagerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder CreateDefault(IServiceCollection services = null) => new Internal.AssemblyManagerBuilder(services);
    }
}