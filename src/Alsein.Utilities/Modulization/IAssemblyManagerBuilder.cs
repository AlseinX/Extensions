using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyManagerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IServiceCollection Services { get; }
    }
}