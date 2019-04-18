#if NETCOREAPP3_0

using Alsein.Extensions.AspNetCore.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Alsein.Extensions.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <param name="hostServiceProviderFactory"></param>
        /// <returns></returns>
        public static IHostBuilder UseWebStartup<TStartup>(this IHostBuilder hostBuilder, Func<HostBuilderContext, IServiceProvider> hostServiceProviderFactory)
            where TStartup : class, IStartup
        {
            new StartupLoader<TStartup>(hostServiceProviderFactory).Apply(hostBuilder);
            return hostBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <param name="hostServiceProviderFactory"></param>
        /// <returns></returns>
        public static IHostBuilder UseWebStartup<TStartup>(this IHostBuilder hostBuilder, Func<IServiceCollection, HostBuilderContext, IServiceProvider> hostServiceProviderFactory)
            where TStartup : class, IStartup => hostBuilder.UseWebStartup<TStartup>(context =>
                {
                    var services = new ServiceCollection();
                    services.AddSingleton(context.Configuration);
                    services.AddSingleton(context.HostingEnvironment);
                    return hostServiceProviderFactory(services, context);
                });
    }
}

#endif
