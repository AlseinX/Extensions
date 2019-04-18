using System;
using System.Collections.Generic;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainer : IScope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IEnumerable<IModule> Modules { get; }

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