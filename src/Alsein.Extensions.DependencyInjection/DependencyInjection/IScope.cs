using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScope : IResolver, IServiceProvider, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        void AddModule(IModule module);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        bool RemoveModule(IModule module);
    }
}