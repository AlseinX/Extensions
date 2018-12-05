using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class NestingContainerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static INestingContainer BuildNestingContainer(this IEnumerable<ServiceDescriptor> services) => new Internal.NestingContainer(services);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNestingContainer(this IServiceCollection services) => services.AddSingleton<IServiceProviderFactory<IServiceCollection>>(new Internal.NestingContainerFactory());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="services"></param>
        /// <param name="asScope"></param>
        /// <returns></returns>
        public static INestingContainer CreateNestedContainer(this IServiceProvider provider, IEnumerable<ServiceDescriptor> services, bool asScope) =>
            provider.GetRequiredService<INestedContainerFactory>().CreateNestedContainer(services, asScope);
    }
}