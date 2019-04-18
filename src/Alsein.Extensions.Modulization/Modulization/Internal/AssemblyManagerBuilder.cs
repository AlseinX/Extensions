using System.IO;
using System.Reflection;
using Alsein.Extensions.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Alsein.Extensions.Modulization.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal class AssemblyManagerBuilder : ServiceBuilder<IAssemblyManager, AssemblyManager, AssemblyManagerOptions>, IAssemblyManagerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public AssemblyManagerBuilder(IServiceCollection services = null) : base(services) { }
    }
}