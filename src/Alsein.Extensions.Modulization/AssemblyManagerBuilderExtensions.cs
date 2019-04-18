using System;
using System.Collections.Generic;
using System.Reflection;
using Alsein.Extensions.Modulization;

namespace Alsein.Extensions
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
        /// <param name="entry"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithEntryAssembly(this IAssemblyManagerBuilder builder, Assembly entry)
        {
            builder.Configure(option => option.EntryAssembly = entry);
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithDirectories(this IAssemblyManagerBuilder builder, Action<IList<AssemblyDirectory>> action)
        {
            builder.Configure(option => action(option.ExternalDirectories));
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
            builder.Configure(option => action(option.FeatureFilters));
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
            builder.Configure(option => option.ProjectAssemblyFilter = filter);
            return builder;
        }
    }
}