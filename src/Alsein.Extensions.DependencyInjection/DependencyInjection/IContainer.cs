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
    }
}