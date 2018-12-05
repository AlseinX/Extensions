using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface INestedContainerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="asScope"></param>
        /// <returns></returns>
        INestingContainer CreateNestedContainer(IEnumerable<ServiceDescriptor> services, bool asScope);
    }
}