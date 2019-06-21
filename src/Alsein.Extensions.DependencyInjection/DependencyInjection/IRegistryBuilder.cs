using System;
using System.Collections.Generic;
using Alsein.Extensions.Patterns;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRegistryBuilder : IBuilder<(IEnumerable<object>, IRegistry)>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        IRegistryBuilder InjectProperties(bool enabled = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRegistryBuilder To(object key);
    }
}