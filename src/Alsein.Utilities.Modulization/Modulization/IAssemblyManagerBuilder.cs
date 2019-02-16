using Alsein.Utilities.Patterns;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyManagerBuilder : IServiceBuilder<IAssemblyManager, AssemblyManagerOptions>
    {
    }
}