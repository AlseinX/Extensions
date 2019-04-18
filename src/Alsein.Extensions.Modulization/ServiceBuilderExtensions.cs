using Alsein.Extensions.Patterns;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIService"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TIService Build<TIService, TOptions>(this IServiceBuilder<TIService, TOptions> builder)
            where TIService : class
            where TOptions : class
            => builder.Services.BuildServiceProvider().GetRequiredService<TIService>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIService"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="serviceProviderFactory"></param>
        /// <returns></returns>
        public static TIService Build<TIService, TOptions, TContainerBuilder>(this IServiceBuilder<TIService, TOptions> builder, IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
            where TIService : class
            where TOptions : class
            => serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(builder.Services)).GetRequiredService<TIService>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIService"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceBuilder<TIService, TOptions> Configure<TIService, TOptions>(this IServiceBuilder<TIService, TOptions> builder, Action<TOptions> configureOptions)
            where TIService : class
            where TOptions : class
        {
            builder.Services.Configure(configureOptions);
            return builder;
        }
    }
}
