using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyManagerBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithDirectories(this IAssemblyManagerBuilder builder, Action<IList<AssemblyDirectory>> action)
        {
            builder.Services.Configure<AssemblyManagerOptions>(option => action(option.ExternalDirectories));
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithFeatureFilters(this IAssemblyManagerBuilder builder, Action<IDictionary<string, Func<Type, bool>>> action)
        {
            builder.Services.Configure<AssemblyManagerOptions>(option => action(option.FeatureFilters));
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithProjectAssemblyFilter(this IAssemblyManagerBuilder builder, Func<Assembly, bool> filter)
        {
            builder.Services.Configure<AssemblyManagerOptions>(option => option.ProjectAssemblyFilter = filter);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAssemblyManager Build(this IAssemblyManagerBuilder builder) => builder.Services.BuildServiceProvider().GetRequiredService<IAssemblyManager>();
    }
}