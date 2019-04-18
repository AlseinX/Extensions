using Alsein.Extensions.Patterns;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Extensions.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyManagerBuilder : IServiceBuilder<IAssemblyManager, AssemblyManagerOptions>
    {
    }
}