using System;
using System.Collections.Generic;
using Alsein.Extensions.Patterns;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModuleBuilder : IBuilder<IModule>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IDictionary<object, IRegistry> Registries { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IModuleBuilder Register(Action<IConstructorRegistryBuilder> action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IModuleBuilder Register(Action<IFactoryRegistryBuilder> action);
    }
}