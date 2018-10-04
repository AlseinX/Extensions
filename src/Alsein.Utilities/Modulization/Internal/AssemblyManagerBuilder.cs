using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Alsein.Utilities.Modulization.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal class AssemblyManagerBuilder : IAssemblyManagerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyManagerBuilder(IServiceCollection services = null)
        {
            Services = services ?? new ServiceCollection();
            Services.AddSingleton<IAssemblyManager, AssemblyManager>();
        }
    }
}