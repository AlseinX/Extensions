using System.IO;
using System.Reflection;
using Alsein.Utilities.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Alsein.Utilities.Modulization.Internal
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